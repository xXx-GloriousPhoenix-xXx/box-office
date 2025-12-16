using BoxOffice.DAL.Models.Entities;

namespace BoxOffice.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable, IAsyncDisposable
    {
        IBaseRepository<Author> Authors { get; }
        IBaseRepository<Booking> Bookings { get; }
        IBaseRepository<Customer> Customers { get; }
        IBaseRepository<Genre> Genres { get; }
        IBaseRepository<Poster> Posters { get; }
        IBaseRepository<Ticket> Tickets { get; }
        IBaseRepository<TicketInfo> TicketInfos { get; }
        IBaseRepository<Transaction> Transactions { get; }

        Task<int> CompleteAsync(CancellationToken ct = default);
        Task BeginTransactionAsync(CancellationToken ct = default);
        Task CommitTransactionAsync(CancellationToken ct = default);
        Task RollbackTransactionAsync(CancellationToken ct = default);
    }
}
