using BoxOffice.BLL.DTO;

namespace BoxOffice.BLL.Interfaces
{
    public interface IBookingService
    {
        Task<BookingDetailsDto?> GetBookingDetailsAsync(string bookingToken, CancellationToken ct = default);
        Task<IEnumerable<BookingDto>> GetCustomerBookingsAsync(Guid customerId, CancellationToken ct = default);
        Task<bool> ValidateBookingAsync(string bookingToken, CancellationToken ct = default);
        Task<bool> CancelExpiredBookingsAsync(CancellationToken ct = default);
    }
}