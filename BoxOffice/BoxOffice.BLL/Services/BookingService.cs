using AutoMapper;
using BoxOffice.BLL.DTO;
using BoxOffice.BLL.Interfaces;
using BoxOffice.DAL.Enums;
using BoxOffice.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace BoxOffice.BLL.Services
{
    public class BookingService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<BookingService> logger) : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<BookingService> _logger = logger;

        public async Task<BookingDetailsDto?> GetBookingDetailsAsync(string bookingToken, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.BookingRepository.GetByTokenAsync(bookingToken, ct);

            if (booking == null)
                return null;

            var dto = _mapper.Map<BookingDetailsDto>(booking);

            if (booking.Tickets != null)
            {
                foreach (var ticket in booking.Tickets)
                {
                    await _unitOfWork.Tickets.GetByIdAsync(
                        ticket.Id,
                        ct,
                        t => t.TicketInfo!,
                        t => t.Poster!);
                }
            }

            return dto;
        }

        public async Task<IEnumerable<BookingDto>> GetCustomerBookingsAsync(Guid customerId, CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository
                .GetCustomerBookingsAsync(customerId, ct);

            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }

        public async Task<bool> ValidateBookingAsync(string bookingToken, CancellationToken ct = default)
        {
            var booking = await _unitOfWork.BookingRepository.GetByTokenAsync(bookingToken, ct);

            if (booking == null)
            {
                _logger.LogWarning("Booking not found: {BookingToken}", bookingToken);
                return false;
            }

            if (booking.Status != BookingStatus.Active)
            {
                _logger.LogWarning("Booking is not active: {BookingToken}, Status: {Status}",
                    bookingToken, booking.Status);
                return false;
            }

            if (booking.ExpiresAt <= DateTime.UtcNow)
            {
                _logger.LogWarning("Booking has expired: {BookingToken}, Expired at: {ExpiresAt}",
                    bookingToken, booking.ExpiresAt);
                return false;
            }

            var tickets = await _unitOfWork.Tickets.FindAsync(
                t => t.BookingId == booking.Id, ct);

            var allTicketsValid = tickets.All(t => t.State == TicketState.Booked);

            if (!allTicketsValid)
            {
                _logger.LogWarning("Some tickets in booking {BookingToken} are no longer booked", bookingToken);
                return false;
            }

            _logger.LogInformation("Booking {BookingToken} is valid", bookingToken);
            return true;
        }

        public async Task<bool> CancelExpiredBookingsAsync(CancellationToken ct = default)
        {
            try
            {
                var result = await _unitOfWork.BookingRepository
                    .CancelExpiredBookingsAsync(ct);

                if (result)
                {
                    _logger.LogInformation("Successfully cancelled expired bookings");
                }
                else
                {
                    _logger.LogInformation("No expired bookings to cancel");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling expired bookings");
                return false;
            }
        }
        public async Task<IEnumerable<BookingDto>> GetActiveBookingsAsync(CancellationToken ct = default)
        {
            var bookings = await _unitOfWork.BookingRepository
                .GetActiveBookingsAsync(ct);

            return _mapper.Map<IEnumerable<BookingDto>>(bookings);
        }

        public async Task<bool> ExtendBookingAsync(string bookingToken, int additionalHours, CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync(ct);

            try
            {
                var booking = await _unitOfWork.BookingRepository.GetByTokenAsync(bookingToken, ct);

                if (booking == null || booking.Status != BookingStatus.Active)
                    return false;

                booking.ExpiresAt = booking.ExpiresAt.AddHours(additionalHours);
                _unitOfWork.Bookings.Update(booking);

                var tickets = await _unitOfWork.Tickets.FindAsync(
                    t => t.BookingId == booking.Id, ct);

                foreach (var ticket in tickets)
                {
                    ticket.BookedUntil = booking.ExpiresAt;
                    _unitOfWork.Tickets.Update(ticket);
                }

                await _unitOfWork.CompleteAsync(ct);
                await _unitOfWork.CommitTransactionAsync(ct);

                _logger.LogInformation(
                    "Booking {BookingToken} extended by {AdditionalHours} hours, new expiry: {ExpiresAt}",
                    bookingToken, additionalHours, booking.ExpiresAt);

                return true;
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                _logger.LogError(ex, "Error extending booking {BookingToken}", bookingToken);
                return false;
            }
        }
    }
}