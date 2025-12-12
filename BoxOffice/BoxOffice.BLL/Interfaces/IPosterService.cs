using BoxOffice.BLL.DTO;

namespace BoxOffice.BLL.Interfaces
{
    public interface IPosterService
    {
        Task<IEnumerable<PosterDto>> SearchPostersAsync(SearchPostersDto searchDto, CancellationToken ct = default);
        Task<PosterDetailsDto?> GetPosterDetailsAsync(Guid posterId, CancellationToken ct = default);
        Task<PosterDto> CreatePosterAsync(CreatePosterDto createDto, CancellationToken ct = default);
        Task<PosterDto> UpdatePosterAsync(Guid posterId, UpdatePosterDto updateDto, CancellationToken ct = default);
        Task<bool> DeletePosterAsync(Guid posterId, CancellationToken ct = default);
        Task<IEnumerable<PosterDto>> GetUpcomingPostersAsync(int daysAhead = 30, CancellationToken ct = default);
    }
}