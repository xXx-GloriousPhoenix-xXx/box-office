using BoxOffice.DAL.Entities;

namespace BoxOffice.DAL.Interfaces
{
    public interface ITicketRepository : IBaseRepository<Ticket>
    {
        Task<IEnumerable<Ticket>> GetAvailableTicketsAsync(Guid posterId, CancellationToken ct = default);
        Task<IEnumerable<Ticket>> GetTicketsByPosterAsync(Guid posterId, CancellationToken ct = default);
        Task<Ticket?> GetBySeatNumberAsync(Guid posterId, string seatNumber, CancellationToken ct = default);
        Task<IEnumerable<Ticket>> GetExpiredBookingsAsync(CancellationToken ct = default);
        Task<IEnumerable<Ticket>> GetCustomerTicketsAsync(Guid customerId, CancellationToken ct = default);
        Task<bool> AreSeatsAvailableAsync(Guid posterId, IEnumerable<string> seatNumbers, CancellationToken ct = default);
        Task<IEnumerable<string>> GetOccupiedSeatsAsync(Guid posterId, CancellationToken ct = default);
        Task<int> GetAvailableTicketsCountAsync(Guid posterId, CancellationToken ct = default);
    }
}