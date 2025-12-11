using BoxOffice.DAL.Entities;

namespace BoxOffice.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IBaseRepository<Author> Authors { get; }
        IBaseRepository<Booking> Bookings { get; }
        IBaseRepository<Customer> Customers { get; }
        IBaseRepository<Poster> Posters { get; }
        IBaseRepository<Ticket> Tickets { get; }
        IBaseRepository<TicketInfo> TicketInfos { get; }
        IBaseRepository<Transaction> Transactions { get; }

        IPosterRepository PosterRepository { get; }
        ITicketRepository TicketRepository { get; }
        IBookingRepository BookingRepository { get; }

        Task<int> CompleteAsync(CancellationToken ct = default);
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitTransactionAsync(CancellationToken ct = default);
        Task RollbackTransactionAsync(CancellationToken ct = default);
    }
}
