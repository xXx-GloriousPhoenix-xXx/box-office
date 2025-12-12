using BoxOffice.DAL.Entities;

namespace BoxOffice.DAL.Interfaces
{
    public interface IBookingRepository : IBaseRepository<Booking>
    {
        Task<Booking?> GetByTokenAsync(string bookingToken, CancellationToken ct = default);
        Task<IEnumerable<Booking>> GetCustomerBookingsAsync(Guid customerId, CancellationToken ct = default);
        Task<IEnumerable<Booking>> GetActiveBookingsAsync(CancellationToken ct = default);
        Task<bool> CancelExpiredBookingsAsync(CancellationToken ct = default);
        Task<Booking?> GetBookingWithDetailsAsync(Guid bookingId, CancellationToken ct = default);
    }
}