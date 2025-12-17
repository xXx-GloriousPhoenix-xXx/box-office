using AutoMapper;
using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.AdditionalDtos.OperationDtos;
using BoxOffice.BLL.DTOs.TicketDtos;
using BoxOffice.BLL.Exceptions;
using BoxOffice.BLL.Services.Interfaces;
using BoxOffice.DAL.Models.Entities;
using BoxOffice.DAL.Models.Enums;
using BoxOffice.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoxOffice.BLL.Services.Implementations
{
    public partial class TicketService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<TicketService> logger)
        : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        [LoggerMessage(LogLevel.Information, "Ticket created: {SeatNumber} (ID: {Id})")]
        partial void LogTicketCreated(string seatNumber, Guid id);

        [LoggerMessage(LogLevel.Information, "Ticket purchased: {SeatNumber} (ID: {Id})")]
        partial void LogTicketPurchased(string seatNumber, Guid id);

        [LoggerMessage(LogLevel.Information, "Ticket purchase cancelled: {SeatNumber} (ID: {Id})")]
        partial void LogTicketPurchaseCancelled(string seatNumber, Guid id);

        [LoggerMessage(LogLevel.Information, "Ticket deleted: {SeatNumber} (ID: {Id})")]
        partial void LogTicketDeleted(string seatNumber, Guid id);

        public async Task<GetTicketDto> AddAsync(CreateTicketDto createDto, CancellationToken ct = default)
        {
            var ticketInfo = await _unitOfWork.TicketInfos.GetByIdAsync(createDto.TicketInfoId, ct);
            if (ticketInfo == null)
            {
                throw new NotFoundException($"TicketInfo with id {createDto.TicketInfoId} not found");
            }

            var seatExists = await _unitOfWork.Tickets
                .ExistsAsync(t => t.TicketInfoId == createDto.TicketInfoId
                    && t.SeatNumber.ToLower() == createDto.SeatNumber.ToLower(), ct);

            if (seatExists)
            {
                throw new ValidationException($"Seat '{createDto.SeatNumber}' is already taken for this event");
            }

            if (ticketInfo.AvailableCount <= 0)
            {
                throw new BusinessException("No available tickets for this event");
            }

            var ticket = _mapper.Map<Ticket>(createDto);
            ticket.TicketState = TicketState.Available;

            ticketInfo.AvailableCount--;
            ticketInfo.TotalCount++;

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                _unitOfWork.Tickets.Add(ticket);
                _unitOfWork.TicketInfos.Update(ticketInfo);

                await _unitOfWork.CompleteAsync(ct);
                await _unitOfWork.CommitTransactionAsync(ct);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw;
            }

            LogTicketCreated(ticket.SeatNumber, ticket.Id);

            return _mapper.Map<GetTicketDto>(ticket);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var ticket = await _unitOfWork.Tickets
                .GetByIdAsync(id, ct,
                    t => t.Transactions,
                    t => t.TicketInfo);

            if (ticket is null)
            {
                throw new NotFoundException($"Ticket with id {id} not found");
            }

            if (ticket.Transactions.Count > 0)
            {
                throw new BusinessException(
                    $"Cannot delete ticket with {ticket.Transactions.Count} transactions. " +
                    "Delete related transactions first.");
            }

            if (ticket.TicketState == TicketState.Sold || ticket.TicketState == TicketState.Booked)
            {
                throw new BusinessException(
                    $"Cannot delete ticket in '{ticket.TicketState}' state. " +
                    "Cancel purchase or booking first.");
            }

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                if (ticket.TicketState == TicketState.Available && ticket.TicketInfo != null)
                {
                    ticket.TicketInfo.AvailableCount++;
                    ticket.TicketInfo.TotalCount--;
                    _unitOfWork.TicketInfos.Update(ticket.TicketInfo);
                }

                _unitOfWork.Tickets.Delete(ticket);

                await _unitOfWork.CompleteAsync(ct);
                await _unitOfWork.CommitTransactionAsync(ct);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw;
            }

            LogTicketDeleted(ticket.SeatNumber, ticket.Id);
        }

        public async Task<PagedResponse<GetTicketDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default)
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

            var query = _unitOfWork.Tickets.AsQueryable();
            var totalCount = await query.CountAsync(ct);

            var tickets = await query
                .Include(t => t.TicketInfo!)
                .ThenInclude(ti => ti!.Poster!)
                .OrderBy(t => t.TicketInfo!.Poster!.Date)
                .ThenBy(t => t.SeatNumber)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync(ct);

            var ticketDtos = _mapper.Map<List<GetTicketDto>>(tickets);

            return new PagedResponse<GetTicketDto>
            {
                Items = ticketDtos,
                CurrentPage = page,
                PageSize = itemsPerPage,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)itemsPerPage)
            };
        }

        public async Task<GetTicketDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var ticket = await _unitOfWork.Tickets
                .GetByIdAsync(id, ct,
                    t => t.TicketInfo,
                    t => t.Customer,
                    t => t.Booking);

            if (ticket is null)
            {
                throw new NotFoundException($"Ticket with id {id} not found");
            }

            return _mapper.Map<GetTicketDto>(ticket);
        }

        public async Task<GetTicketDto> PurchaseAsync(Guid id, PurchaseDto dto, CancellationToken ct = default)
        {
            var ticket = await _unitOfWork.Tickets
                .GetByIdAsync(id, ct,
                    includes: t => t.TicketInfo);

            if (ticket is null)
            {
                throw new NotFoundException($"Ticket with id {id} not found");
            }

            if (ticket.TicketState != TicketState.Available && ticket.TicketState != TicketState.Booked)
            {
                throw new BusinessException(
                    $"Cannot purchase ticket in '{ticket.TicketState}' state. " +
                    "Ticket must be Available or Booked.");
            }

            if (ticket.TicketState == TicketState.Booked && ticket.BookingId.HasValue)
            {
                if (dto.CustomerId != ticket.CustomerId)
                {
                    throw new BusinessException("Ticket is booked by another user");
                }

                var booking = await _unitOfWork.Bookings.GetByIdAsync(ticket.BookingId.Value, ct);
                if (booking == null || booking.State != BookingState.Active || booking.ExpiresAt < DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    throw new BusinessException("Booking is expired or invalid");
                }
            }

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                ticket.TicketState = TicketState.Sold;
                ticket.SoldDate = DateOnly.FromDateTime(DateTime.UtcNow);
                ticket.CustomerId = dto.CustomerId;

                if (ticket.TicketInfo != null)
                {
                    if (ticket.TicketState == TicketState.Booked)
                    {
                        ticket.TicketInfo.BookedCount--;
                    }
                    ticket.TicketInfo.SoldCount++;
                    ticket.TicketInfo.AvailableCount--;
                    _unitOfWork.TicketInfos.Update(ticket.TicketInfo);
                }

                if (ticket.BookingId.HasValue)
                {
                    var booking = await _unitOfWork.Bookings.GetByIdAsync(ticket.BookingId.Value, ct);
                    if (booking != null)
                    {
                        booking.State = BookingState.Completed;
                        _unitOfWork.Bookings.Update(booking);
                    }
                }

                _unitOfWork.Tickets.Update(ticket);

                await _unitOfWork.CompleteAsync(ct);
                await _unitOfWork.CommitTransactionAsync(ct);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw;
            }

            LogTicketPurchased(ticket.SeatNumber, ticket.Id);

            return _mapper.Map<GetTicketDto>(ticket);
        }

        public async Task<GetTicketDto> CancelPurchaseAsync(Guid id, CancelPurcaseDto dto, CancellationToken ct = default)
        {
            var ticket = await _unitOfWork.Tickets
                .GetByIdAsync(id, ct,
                    t => t.TicketInfo,
                    t => t.Transactions);

            if (ticket is null)
            {
                throw new NotFoundException($"Ticket with id {id} not found");
            }

            if (ticket.TicketState != TicketState.Sold)
            {
                throw new BusinessException($"Cannot cancel purchase for ticket in '{ticket.TicketState}' state.");
            }

            if (ticket.Transactions.Count > 0)
            {
                throw new BusinessException(
                    $"Cannot cancel purchase for ticket with {ticket.Transactions.Count} transactions. " +
                    "Refund or delete transactions first.");
            }

            if (dto.CustomerId != ticket.CustomerId)
            {
                throw new BusinessException("Cannot cancel purchase for ticket bought by another customer");
            }

            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                ticket.TicketState = TicketState.Available;
                ticket.SoldDate = null;
                ticket.CustomerId = null;

                if (ticket.TicketInfo != null)
                {
                    ticket.TicketInfo.SoldCount--;
                    ticket.TicketInfo.AvailableCount++;
                    _unitOfWork.TicketInfos.Update(ticket.TicketInfo);
                }

                _unitOfWork.Tickets.Update(ticket);

                await _unitOfWork.CompleteAsync(ct);
                await _unitOfWork.CommitTransactionAsync(ct);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw;
            }

            LogTicketPurchaseCancelled(ticket.SeatNumber, ticket.Id);

            return _mapper.Map<GetTicketDto>(ticket);
        }
    }
}
