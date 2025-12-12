using BoxOffice.DAL.Context;
using BoxOffice.DAL.Entities;
using BoxOffice.DAL.Enums;
using BoxOffice.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoxOffice.DAL.Repository
{
    public class TicketRepository(BoxOfficeDbContext context) : BaseRepository<Ticket>(context), ITicketRepository
    {
        public async Task<IEnumerable<Ticket>> GetAvailableTicketsAsync(Guid posterId, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(t => t.TicketInfo)
                .Include(t => t.Poster)
                .Where(t => t.PosterId == posterId && t.State == TicketState.Available)
                .OrderBy(t => t.SeatNumber)
                .ToListAsync(ct);
        }

        public async Task<Ticket?> GetBySeatNumberAsync(Guid posterId, string seatNumber, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(t => t.TicketInfo)
                .Include(t => t.Poster)
                .Include(t => t.Customer)
                .Include(t => t.Booking)
                .FirstOrDefaultAsync(t => t.PosterId == posterId &&
                    t.SeatNumber.Equals(seatNumber, StringComparison.CurrentCultureIgnoreCase), ct);
        }

        public async Task<IEnumerable<Ticket>> GetCustomerTicketsAsync(Guid customerId, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(t => t.TicketInfo)
                .Include(t => t.Poster)
                .Include(t => t.Booking)
                .Where(t => t.CustomerId == customerId)
                .OrderByDescending(t => t.BookedUntil)
                .ThenBy(t => t.SeatNumber)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Ticket>> GetExpiredBookingsAsync(CancellationToken ct = default)
        {
            return await _dbSet
                .Include(t => t.TicketInfo)
                .Include(t => t.Poster)
                .Include(t => t.Booking)
                .Include(t => t.Customer)
                .Where(t => t.State == TicketState.Booked &&
                    t.BookedUntil.HasValue &&
                    t.BookedUntil.Value <= DateTime.UtcNow)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByPosterAsync(Guid posterId, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(t => t.TicketInfo)
                .Include(t => t.Customer)
                .Include(t => t.Booking)
                .Where(t => t.PosterId == posterId)
                .OrderBy(t => t.SeatNumber)
                .ToListAsync(ct);
        }


        public async Task<bool> AreSeatsAvailableAsync(Guid posterId, IEnumerable<string> seatNumbers, CancellationToken ct = default)
        {
            var occupiedSeats = await _dbSet
                .Where(t => t.PosterId == posterId &&
                    seatNumbers.Contains(t.SeatNumber) &&
                    (t.State == TicketState.Booked || t.State == TicketState.Sold))
                .Select(t => t.SeatNumber)
                .ToListAsync(ct);

            return occupiedSeats.Count == 0;
        }

        public async Task<IEnumerable<string>> GetOccupiedSeatsAsync(Guid posterId, CancellationToken ct = default)
        {
            return await _dbSet
                .Where(t => t.PosterId == posterId &&
                    (t.State == TicketState.Booked || t.State == TicketState.Sold))
                .Select(t => t.SeatNumber)
                .ToListAsync(ct);
        }

        public async Task<int> GetAvailableTicketsCountAsync(Guid posterId, CancellationToken ct = default)
        {
            return await _dbSet
                .CountAsync(t => t.PosterId == posterId && t.State == TicketState.Available, ct);
        }
    }
}