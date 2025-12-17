using AutoMapper;
using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.BookingDtos;
using BoxOffice.BLL.Exceptions;
using BoxOffice.BLL.Services.Interfaces;
using BoxOffice.DAL.Models.Entities;
using BoxOffice.DAL.Models.Enums;
using BoxOffice.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoxOffice.BLL.Services.Implementations
{
    public partial class BookingService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<BookingService> logger)
        : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        [LoggerMessage(LogLevel.Information, "Booking created: Token {Token} (ID: {Id})")]
        partial void LogBookingCreated(string token, Guid id);

        [LoggerMessage(LogLevel.Information, "Booking updated: ID: {Id}, State: {State}")]
        partial void LogBookingUpdated(Guid id, BookingState state);

        [LoggerMessage(LogLevel.Information, "Booking deleted: ID: {Id}")]
        partial void LogBookingDeleted(Guid id);

        public async Task<GetBookingDto> AddAsync(CreateBookingDto createDto, CancellationToken ct = default)
        {
            var ticket = await _unitOfWork.Tickets
                .GetByIdAsync(createDto.TicketId, ct,
                    includes: t => t.TicketInfo!);

            if (ticket is null)
            {
                throw new NotFoundException($"Ticket with id {createDto.TicketId} not found");
            }

            if (ticket.BookingId.HasValue)
            {
                var existingBooking = await _unitOfWork.Bookings.GetByIdAsync(ticket.BookingId.Value, ct);
                if (existingBooking != null && existingBooking.State == BookingState.Active)
                {
                    throw new ValidationException($"Ticket is already booked. Booking ID: {ticket.BookingId}");
                }
            }

            if (ticket.TicketState != TicketState.Available)
            {
                throw new BusinessException(
                    $"Cannot book ticket in '{ticket.TicketState}' state. " +
                    "Ticket must be available for booking.");
            }

            string bookingToken;
            bool tokenExists;
            int attempts = 0;
            const int maxAttempts = 5;

            do
            {
                bookingToken = GenerateBookingToken();
                tokenExists = await _unitOfWork.Bookings
                    .ExistsAsync(b => b.BookingToken == bookingToken, ct);
                attempts++;
            } while (tokenExists && attempts < maxAttempts);

            if (tokenExists)
            {
                throw new BusinessException("Failed to generate unique booking token. Please try again.");
            }

            var booking = new Booking
            {
                BookedAt = DateOnly.FromDateTime(DateTime.UtcNow),
                ExpiresAt = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(48)), // 48 hours booking
                BookingToken = bookingToken,
                State = BookingState.Active,
                TicketId = createDto.TicketId
            };

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                _unitOfWork.Bookings.Add(booking);
                await _unitOfWork.CompleteAsync(ct); // Save to get booking Id

                ticket.BookingId = booking.Id;
                ticket.TicketState = TicketState.Booked;
                _unitOfWork.Tickets.Update(ticket);

                if (ticket.TicketInfo != null)
                {
                    ticket.TicketInfo.BookedCount++;
                    ticket.TicketInfo.AvailableCount--;
                    _unitOfWork.TicketInfos.Update(ticket.TicketInfo);
                }

                await _unitOfWork.CompleteAsync(ct);
                await _unitOfWork.CommitTransactionAsync(ct);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw;
            }

            LogBookingCreated(booking.BookingToken, booking.Id);

            return _mapper.Map<GetBookingDto>(booking);
        }

        public async Task<GetBookingDto> BookAsync(Guid ticketId, CancellationToken ct = default)
        {
            var createDto = new CreateBookingDto { TicketId = ticketId };
            return await AddAsync(createDto, ct);
        }

        public async Task CancelBookingAsync(Guid ticketId, CancellationToken ct = default)
        {
            var ticket = await _unitOfWork.Tickets
                .GetByIdAsync(ticketId, ct,
                    t => t.Booking,
                    t => t.TicketInfo);

            if (ticket == null)
            {
                throw new NotFoundException($"Ticket with id {ticketId} not found");
            }

            if (!ticket.BookingId.HasValue || ticket.Booking == null)
            {
                throw new ValidationException($"Ticket is not booked");
            }

            var booking = ticket.Booking;

            if (booking.State != BookingState.Active)
            {
                throw new BusinessException($"Cannot cancel booking in '{booking.State}' state.");
            }

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                booking.State = BookingState.Cancelled;
                _unitOfWork.Bookings.Update(booking);

                ticket.TicketState = TicketState.Available;
                ticket.BookingId = null;
                _unitOfWork.Tickets.Update(ticket);

                if (ticket.TicketInfo != null)
                {
                    ticket.TicketInfo.BookedCount--;
                    ticket.TicketInfo.AvailableCount++;
                    _unitOfWork.TicketInfos.Update(ticket.TicketInfo);
                }

                await _unitOfWork.CompleteAsync(ct);
                await _unitOfWork.CommitTransactionAsync(ct);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw;
            }

            LogBookingUpdated(booking.Id, booking.State);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.Bookings
                .GetByIdAsync(id, ct,
                    includes: b => b.Ticket!);

            if (booking is null)
            {
                throw new NotFoundException($"Booking with id {id} not found");
            }

            if (booking.State == BookingState.Active)
            {
                throw new BusinessException(
                    "Cannot delete active booking. Cancel it first.");
            }

            if (booking.Ticket != null)
            {
                await _unitOfWork.BeginTransactionAsync(ct);
                try
                {
                    if (booking.Ticket.BookingId == booking.Id)
                    {
                        booking.Ticket.BookingId = null;
                        _unitOfWork.Tickets.Update(booking.Ticket);
                    }

                    _unitOfWork.Bookings.Delete(booking);

                    await _unitOfWork.CompleteAsync(ct);
                    await _unitOfWork.CommitTransactionAsync(ct);
                }
                catch
                {
                    await _unitOfWork.RollbackTransactionAsync(ct);
                    throw;
                }
            }
            else
            {
                _unitOfWork.Bookings.Delete(booking);
                await _unitOfWork.CompleteAsync(ct);
            }

            LogBookingDeleted(booking.Id);
        }

        public async Task<PagedResponse<GetBookingDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default)
        {
            if (page < 1)
            {
                page = 1;
            }
            if (itemsPerPage < 1)
            {
                itemsPerPage = 10;
            }
            if (itemsPerPage > 100)
            {
                itemsPerPage = 100;
            }

            var query = _unitOfWork.Bookings.AsQueryable()
                .Include(b => b.Ticket!);

            var totalCount = await query.CountAsync(ct);

            var bookings = await query
                .OrderByDescending(b => b.BookedAt)
                .ThenByDescending(b => b.Id)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync(ct);

            var bookingDtos = _mapper.Map<List<GetBookingDto>>(bookings);

            return new PagedResponse<GetBookingDto>
            {
                Items = bookingDtos,
                CurrentPage = page,
                PageSize = itemsPerPage,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)itemsPerPage)
            };
        }

        public async Task<GetBookingDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.Bookings
                .GetByIdAsync(id, ct,
                    includes: b => b.Ticket!);

            if (booking is null)
            {
                throw new NotFoundException($"Booking with id {id} not found");
            }

            return _mapper.Map<GetBookingDto>(booking);
        }

        public async Task<int> ExpireOldBookingsAsync(CancellationToken ct = default)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var expiredBookings = await _unitOfWork.Bookings
                .FindAsync(b => b.State == BookingState.Active
                    && b.ExpiresAt < today, ct);

            if (!expiredBookings.Any())
            {
                return 0;
            }

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                var expiredCount = 0;

                foreach (var booking in expiredBookings)
                {
                    booking.State = BookingState.Expired;
                    _unitOfWork.Bookings.Update(booking);

                    var ticket = await _unitOfWork.Tickets
                        .GetByIdAsync(booking.TicketId, ct,
                            includes: t => t.TicketInfo!);

                    if (ticket != null && ticket.BookingId == booking.Id)
                    {
                        ticket.TicketState = TicketState.Available;
                        ticket.BookingId = null;
                        _unitOfWork.Tickets.Update(ticket);

                        if (ticket.TicketInfo != null)
                        {
                            ticket.TicketInfo.BookedCount--;
                            ticket.TicketInfo.AvailableCount++;
                            _unitOfWork.TicketInfos.Update(ticket.TicketInfo);
                        }
                    }

                    expiredCount++;
                }

                await _unitOfWork.CompleteAsync(ct);
                await _unitOfWork.CommitTransactionAsync(ct);

                logger.LogInformation("Expired {Count} old bookings", expiredCount);
                return expiredCount;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw;
            }
        }

        public async Task<GetBookingDto> GetByTokenAsync(string token, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.Bookings
                .AsQueryable()
                .Include(b => b.Ticket!)
                .FirstOrDefaultAsync(b => b.BookingToken == token, ct);

            if (booking is null)
            {
                throw new NotFoundException($"Booking with token '{token}' not found");
            }

            return _mapper.Map<GetBookingDto>(booking);
        }

        private static string GenerateBookingToken()
        {
            return Guid.NewGuid().ToString("N")[..8].ToUpper();
        }
    }
}
