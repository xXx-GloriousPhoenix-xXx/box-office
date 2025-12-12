using BoxOffice.DAL.Context;
using BoxOffice.DAL.Entities;
using BoxOffice.DAL.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace BoxOffice.DAL.Repository
{
    public class UnitOfWork(BoxOfficeDbContext context) : IUnitOfWork
    {
        private readonly BoxOfficeDbContext _context = context;
        private IDbContextTransaction? _currentTransaction;

        private IBaseRepository<Author>? _authors;
        private IBaseRepository<Poster>? _posters;
        private IBaseRepository<TicketInfo>? _ticketInfos;
        private IBaseRepository<Ticket>? _tickets;
        private IBaseRepository<Customer>? _customers;
        private IBaseRepository<Booking>? _bookings;
        private IBaseRepository<Transaction>? _transactions;

        private IPosterRepository? _posterRepository;
        private ITicketRepository? _ticketRepository;
        private IBookingRepository? _bookingRepository;

        public IBaseRepository<Author> Authors =>
            _authors ??= new BaseRepository<Author>(_context);

        public IBaseRepository<Poster> Posters =>
            _posters ??= new BaseRepository<Poster>(_context);

        public IBaseRepository<TicketInfo> TicketInfos =>
            _ticketInfos ??= new BaseRepository<TicketInfo>(_context);

        public IBaseRepository<Ticket> Tickets =>
            _tickets ??= new BaseRepository<Ticket>(_context);

        public IBaseRepository<Customer> Customers =>
            _customers ??= new BaseRepository<Customer>(_context);

        public IBaseRepository<Booking> Bookings =>
            _bookings ??= new BaseRepository<Booking>(_context);

        public IBaseRepository<Transaction> Transactions =>
            _transactions ??= new BaseRepository<Transaction>(_context);
        public IPosterRepository PosterRepository =>
            _posterRepository ??= new PosterRepository(_context);

        public ITicketRepository TicketRepository =>
            _ticketRepository ??= new TicketRepository(_context);

        public IBookingRepository BookingRepository =>
            _bookingRepository ??= new BookingRepository(_context);

        public async Task<int> CompleteAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            _currentTransaction = await _context.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No active transaction");

            await _currentTransaction.CommitAsync(ct);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        public async Task RollbackTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTransaction == null)
                throw new InvalidOperationException("No active transaction");

            await _currentTransaction.RollbackAsync(ct);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                    _currentTransaction?.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async ValueTask DisposeAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
            }
            await _context.DisposeAsync();
            Dispose(false);
            GC.SuppressFinalize(this);
        }
    }
}
