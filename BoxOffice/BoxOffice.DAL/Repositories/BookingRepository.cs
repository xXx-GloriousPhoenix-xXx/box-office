using BoxOffice.DAL.Context;
using BoxOffice.DAL.Entities;
using BoxOffice.DAL.Enums;
using BoxOffice.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoxOffice.DAL.Repository
{
    public class BookingRepository(BoxOfficeDbContext context) : BaseRepository<Booking>(context), IBookingRepository
    {
        public async Task<bool> CancelExpiredBookingsAsync(CancellationToken ct = default)
        {
            var expiredBookings = await _dbSet
                .Include(b => b.Tickets)
                .Where(b => b.Status == BookingStatus.Active && b.ExpiresAt <= DateTime.UtcNow)
                .ToListAsync(ct);

            if (!expiredBookings.Any())
                return false;

            foreach (var booking in expiredBookings)
            {
                booking.Status = BookingStatus.Expired;

                foreach (var ticket in booking.Tickets)
                {
                    ticket.State = TicketState.Available;
                    ticket.BookingId = null;
                    ticket.CustomerId = null;
                    ticket.BookedUntil = null;
                    _dbContext.Entry(ticket).State = EntityState.Modified;
                }

                _dbContext.Entry(booking).State = EntityState.Modified;
            }

            await _dbContext.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IEnumerable<Booking>> GetActiveBookingsAsync(CancellationToken ct = default)
        {
            return await _dbSet
                .Include(b => b.Customer)
                .Include(b => b.Tickets)
                .Where(b => b.Status == BookingStatus.Active && b.ExpiresAt > DateTime.UtcNow)
                .ToListAsync(ct);
        }

        public async Task<Booking?> GetByTokenAsync(string bookingToken, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(b => b.Customer)
                .Include(b => b.Tickets)
                    .ThenInclude(t => t.TicketInfo)
                .Include(b => b.Tickets)
                    .ThenInclude(t => t.Poster)
                .FirstOrDefaultAsync(b => b.BookingToken == bookingToken, ct);
        }

        public async Task<IEnumerable<Booking>> GetCustomerBookingsAsync(Guid customerId, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(b => b.Tickets)
                    .ThenInclude(t => t.TicketInfo)
                .Include(b => b.Tickets)
                    .ThenInclude(t => t.Poster)
                .Where(b => b.CustomerId == customerId)
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync(ct);
        }

        public async Task<Booking?> GetBookingWithDetailsAsync(Guid bookingId, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(b => b.Customer)
                .Include(b => b.Tickets)
                    .ThenInclude(t => t.TicketInfo)
                .Include(b => b.Tickets)
                    .ThenInclude(t => t.Poster)
                .Include(b => b.Tickets)
                    .ThenInclude(t => t.TicketInfo)
                    .ThenInclude(ti => ti!.Poster)
                .FirstOrDefaultAsync(b => b.Id == bookingId, ct);
        }
    }
}