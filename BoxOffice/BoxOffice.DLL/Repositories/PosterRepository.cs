using BoxOffice.DAL.Context;
using BoxOffice.DAL.Entities;
using BoxOffice.DAL.Enums;
using BoxOffice.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BoxOffice.DAL.Repository
{
    public class PosterRepository(BoxOfficeDbContext context) : BaseRepository<Poster>(context), IPosterRepository
    {
        public async Task<IEnumerable<Poster>> GetPostersWithAvailableTicketsAsync(CancellationToken ct = default)
        {
            return await _dbSet
                .Include(p => p.Author)
                .Include(p => p.TicketInfos)
                .Where(p => p.TicketInfos.Any(ti => ti.AvailableTickets > 0))
                .ToListAsync(ct);
        }

        public async Task<Poster?> GetPosterWithDetailsAsync(Guid posterId, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(p => p.Author)
                .Include(p => p.TicketInfos)
                    .ThenInclude(ti => ti.Tickets)
                .Include(p => p.Tickets)
                .FirstOrDefaultAsync(p => p.Id == posterId, ct);
        }

        public async Task<IEnumerable<Poster>> SearchAsync(
            string? authorName = null,
            string? title = null,
            PosterGenre? genre = null,
            DateOnly? dateFrom = null,
            DateOnly? dateTo = null,
            CancellationToken ct = default)
        {
            IQueryable<Poster> query = _dbSet
                .Include(p => p.Author)
                .Include(p => p.TicketInfos);

            if (!string.IsNullOrWhiteSpace(authorName))
            {
                query = query.Where(p => p.Author != null &&
                    p.Author.Name.Contains(authorName));
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(p => p.Name.Contains(title));
            }

            if (genre.HasValue)
            {
                query = query.Where(p => p.Genres.Contains(genre.Value));
            }

            if (dateFrom.HasValue)
            {
                query = query.Where(p => p.ReleaseDate >= dateFrom.Value);
            }

            if (dateTo.HasValue)
            {
                query = query.Where(p => p.ReleaseDate <= dateTo.Value);
            }

            return await query.ToListAsync(ct);
        }

        public async Task<IEnumerable<Poster>> GetUpcomingPostersAsync(int daysAhead = 30, CancellationToken ct = default)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var futureDate = today.AddDays(daysAhead);

            return await _dbSet
                .Include(p => p.Author)
                .Include(p => p.TicketInfos)
                .Where(p => p.ReleaseDate >= today && p.ReleaseDate <= futureDate)
                .OrderBy(p => p.ReleaseDate)
                .ThenBy(p => p.Name)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Poster>> GetPostersByAuthorAsync(Guid authorId, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(p => p.Author)
                .Include(p => p.TicketInfos)
                .Where(p => p.AuthorId == authorId)
                .OrderByDescending(p => p.ReleaseDate)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Poster>> GetRecentPostersAsync(int count = 10, CancellationToken ct = default)
        {
            return await _dbSet
                .Include(p => p.Author)
                .Include(p => p.TicketInfos)
                .OrderByDescending(p => p.ReleaseDate)
                .Take(count)
                .ToListAsync(ct);
        }
    }
}