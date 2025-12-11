using BoxOffice.DAL.Entities;
using BoxOffice.DAL.Enums;

namespace BoxOffice.DAL.Interfaces
{
    public interface IPosterRepository : IBaseRepository<Poster>
    {
        Task<IEnumerable<Poster>> SearchAsync(
            string? authorName = null,
            string? title = null,
            PosterGenre? genre = null,
            DateOnly? dateFrom = null,
            DateOnly? dateTo = null,
            CancellationToken ct = default);

        Task<IEnumerable<Poster>> GetPostersWithAvailableTicketsAsync(CancellationToken ct = default);
        Task<Poster?> GetPosterWithDetailsAsync(Guid posterId, CancellationToken ct = default);
        Task<IEnumerable<Poster>> GetUpcomingPostersAsync(int daysAhead = 30, CancellationToken ct = default);
        Task<IEnumerable<Poster>> GetPostersByAuthorAsync(Guid authorId, CancellationToken ct = default);
        Task<IEnumerable<Poster>> GetRecentPostersAsync(int count = 10, CancellationToken ct = default);
    }
}