using AutoMapper;
using BoxOffice.BLL.DTO;
using BoxOffice.BLL.Interfaces;
using BoxOffice.DAL.Entities;
using BoxOffice.DAL.Enums;
using BoxOffice.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace BoxOffice.BLL.Services
{
    public class TicketService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<TicketService> logger) : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<TicketService> _logger = logger;

        public async Task<IEnumerable<TicketDto>> GetAvailableTicketsAsync(
            Guid posterId,
            CancellationToken ct = default)
        {
            var tickets = await _unitOfWork.TicketRepository
                .GetAvailableTicketsAsync(posterId, ct);

            return _mapper.Map<IEnumerable<TicketDto>>(tickets);
        }

        public async Task<TicketDetailsDto?> GetTicketDetailsAsync(
            Guid ticketId,
            CancellationToken ct = default)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(
                ticketId,
                ct,
                t => t.Poster!,
                t => t.TicketInfo!,
                t => t.Customer!,
                t => t.Booking!);

            if (ticket == null)
                return null;

            return _mapper.Map<TicketDetailsDto>(ticket);
        }

        public async Task<BookingResultDto> BookTicketsAsync(
            BookTicketsDto bookDto,
            CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync(ct);

            try
            {
                Customer customer;
                var existingCustomer = await _unitOfWork.Customers
                    .FindAsync(c => c.Email == bookDto.CustomerEmail, ct)
                    .ContinueWith(t => t.Result.FirstOrDefault());

                if (existingCustomer != null)
                {
                    customer = existingCustomer;
                }
                else
                {
                    customer = new Customer
                    {
                        Name = bookDto.CustomerName,
                        Email = bookDto.CustomerEmail
                    };
                    _unitOfWork.Customers.Add(customer);
                    await _unitOfWork.CompleteAsync(ct);
                }

                var areAvailable = await _unitOfWork.TicketRepository
                    .AreSeatsAvailableAsync(bookDto.PosterId, bookDto.SeatNumbers, ct);

                if (!areAvailable)
                    throw new InvalidOperationException("Some seats are not available");

                var tickets = new List<Ticket>();
                foreach (var seatNumber in bookDto.SeatNumbers)
                {
                    var ticket = await _unitOfWork.TicketRepository
                        .GetBySeatNumberAsync(bookDto.PosterId, seatNumber, ct);

                    if (ticket == null)
                        throw new ArgumentException($"Ticket for seat {seatNumber} not found");

                    tickets.Add(ticket);
                }

                var booking = new Booking
                {
                    CustomerId = customer.Id,
                    BookingToken = Guid.NewGuid().ToString("N"),
                    ExpiresAt = DateTime.UtcNow.AddHours(24),
                    Status = BookingStatus.Active,
                    TotalAmount = tickets.Sum(t => t.TicketInfo?.Price ?? 0)
                };

                _unitOfWork.Bookings.Add(booking);
                await _unitOfWork.CompleteAsync(ct);

                foreach (var ticket in tickets)
                {
                    ticket.State = TicketState.Booked;
                    ticket.BookingId = booking.Id;
                    ticket.CustomerId = customer.Id;
                    ticket.BookedUntil = booking.ExpiresAt;

                    if (ticket.TicketInfo != null)
                    {
                        ticket.TicketInfo.BookedTickets++;
                        ticket.TicketInfo.AvailableTickets--;
                        _unitOfWork.TicketInfos.Update(ticket.TicketInfo);
                    }

                    _unitOfWork.Tickets.Update(ticket);
                }

                var transaction = new Transaction
                {
                    CustomerId = customer.Id,
                    TicketId = tickets.First().Id, // Первый билет как ссылка
                    TransactionType = TransactionType.BookingFee,
                    Amount = 0, // Бронирование бесплатно
                    PaymentMethod = "Reservation",
                    TransactionDate = DateTime.UtcNow
                };

                _unitOfWork.Transactions.Add(transaction);

                await _unitOfWork.CompleteAsync(ct);
                await _unitOfWork.CommitTransactionAsync(ct);

                _logger.LogInformation(
                    "Booking created: {BookingToken} for {CustomerEmail} with {TicketCount} tickets",
                    booking.BookingToken, bookDto.CustomerEmail, tickets.Count);

                return new BookingResultDto
                {
                    BookingId = booking.Id,
                    BookingToken = booking.BookingToken,
                    ExpiresAt = booking.ExpiresAt,
                    TotalAmount = booking.TotalAmount,
                    Tickets = _mapper.Map<List<TicketDto>>(tickets)
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw;
            }
        }

        public async Task<SaleResultDto> SellTicketAsync(
            SellTicketDto sellDto,
            CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync(ct);

            try
            {
                var ticket = await _unitOfWork.Tickets.GetByIdAsync(
                    sellDto.TicketId,
                    ct,
                    t => t.TicketInfo!,
                    t => t.Poster!);

                if (ticket == null)
                    throw new ArgumentException($"Ticket with ID {sellDto.TicketId} not found");

                if (ticket.State == TicketState.Sold)
                    throw new InvalidOperationException("Ticket already sold");

                if (ticket.State == TicketState.Booked &&
                    ticket.BookedUntil.HasValue &&
                    ticket.BookedUntil.Value < DateTime.UtcNow)
                    throw new InvalidOperationException("Booking has expired");

                var customer = await _unitOfWork.Customers
                    .GetByIdAsync(sellDto.CustomerId, ct);

                if (customer == null)
                    throw new ArgumentException($"Customer with ID {sellDto.CustomerId} not found");

                // Продаем билет
                ticket.State = TicketState.Sold;
                ticket.SoldDate = DateTime.UtcNow;
                ticket.CustomerId = sellDto.CustomerId;

                if (ticket.TicketInfo != null)
                {
                    ticket.TicketInfo.SoldTickets++;
                    if (ticket.State == TicketState.Booked)
                        ticket.TicketInfo.BookedTickets--;
                    else
                        ticket.TicketInfo.AvailableTickets--;

                    _unitOfWork.TicketInfos.Update(ticket.TicketInfo);
                }

                // Обновляем бронирование, если есть
                if (ticket.BookingId.HasValue)
                {
                    var booking = await _unitOfWork.Bookings
                        .GetByIdAsync(ticket.BookingId.Value, ct);

                    if (booking != null)
                    {
                        // Проверяем, все ли билеты в бронировании проданы
                        var bookingTickets = await _unitOfWork.Tickets.FindAsync(
                            t => t.BookingId == booking.Id, ct);

                        if (bookingTickets.All(t => t.State == TicketState.Sold))
                        {
                            booking.Status = BookingStatus.Completed;
                            _unitOfWork.Bookings.Update(booking);
                        }
                    }
                }

                _unitOfWork.Tickets.Update(ticket);

                // Создаем транзакцию продажи
                var transaction = new Transaction
                {
                    CustomerId = sellDto.CustomerId,
                    TicketId = ticket.Id,
                    TransactionType = TransactionType.Purchase,
                    Amount = ticket.TicketInfo?.Price ?? 0,
                    PaymentMethod = sellDto.PaymentMethod,
                    PaymentReference = sellDto.PaymentReference,
                    TransactionDate = DateTime.UtcNow
                };

                _unitOfWork.Transactions.Add(transaction);
                await _unitOfWork.CompleteAsync(ct);
                await _unitOfWork.CommitTransactionAsync(ct);

                _logger.LogInformation(
                    "Ticket sold: {TicketId} to {CustomerId} for {Amount}",
                    ticket.Id, sellDto.CustomerId, transaction.Amount);

                return new SaleResultDto
                {
                    TicketId = ticket.Id,
                    TransactionId = transaction.Id,
                    Amount = transaction.Amount,
                    SoldDate = ticket.SoldDate.Value
                };
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw;
            }
        }

        public async Task<bool> CancelBookingAsync(
            string bookingToken,
            CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync(ct);

            try
            {
                var booking = await _unitOfWork.BookingRepository
                    .GetByTokenAsync(bookingToken, ct);

                if (booking == null || booking.Status != BookingStatus.Active)
                    return false;

                // Получаем все билеты бронирования
                var tickets = await _unitOfWork.Tickets.FindAsync(
                    t => t.BookingId == booking.Id, ct);

                // Возвращаем билеты в доступные
                foreach (var ticket in tickets)
                {
                    ticket.State = TicketState.Available;
                    ticket.BookingId = null;
                    ticket.CustomerId = null;
                    ticket.BookedUntil = null;

                    if (ticket.TicketInfo != null)
                    {
                        ticket.TicketInfo.BookedTickets--;
                        ticket.TicketInfo.AvailableTickets++;
                        _unitOfWork.TicketInfos.Update(ticket.TicketInfo);
                    }

                    _unitOfWork.Tickets.Update(ticket);
                }

                // Обновляем статус бронирования
                booking.Status = BookingStatus.Cancelled;
                _unitOfWork.Bookings.Update(booking);

                // Создаем транзакцию отмены
                var transaction = new Transaction
                {
                    CustomerId = booking.CustomerId,
                    TicketId = tickets.FirstOrDefault()?.Id ?? Guid.Empty,
                    TransactionType = TransactionType.Cancellation,
                    Amount = 0,
                    PaymentMethod = "System",
                    TransactionDate = DateTime.UtcNow
                };

                _unitOfWork.Transactions.Add(transaction);

                await _unitOfWork.CompleteAsync(ct);
                await _unitOfWork.CommitTransactionAsync(ct);

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw;
            }
        }

        public async Task<bool> ConvertBookingToSaleAsync(
            string bookingToken,
            CancellationToken ct = default)
        {
            var booking = await _unitOfWork.BookingRepository
                .GetByTokenAsync(bookingToken, ct);

            if (booking == null || booking.Status != BookingStatus.Active)
                return false;

            // Получаем все билеты бронирования
            var tickets = await _unitOfWork.Tickets.FindAsync(
                t => t.BookingId == booking.Id, ct);

            // Продаем каждый билет
            foreach (var ticket in tickets)
            {
                var sellDto = new SellTicketDto
                {
                    TicketId = ticket.Id,
                    CustomerId = booking.CustomerId,
                    PaymentMethod = "Card",
                    PaymentReference = $"Booking-{bookingToken}"
                };

                await SellTicketAsync(sellDto, ct);
            }

            return true;
        }

        public async Task<IEnumerable<TicketDto>> GetCustomerTicketsAsync(
            Guid customerId,
            CancellationToken ct = default)
        {
            var tickets = await _unitOfWork.TicketRepository
                .GetCustomerTicketsAsync(customerId, ct);

            return _mapper.Map<IEnumerable<TicketDto>>(tickets);
        }
    }
}