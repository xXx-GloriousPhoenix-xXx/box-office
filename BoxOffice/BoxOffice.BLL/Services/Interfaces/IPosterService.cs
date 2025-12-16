using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.PosterDtos;
using BoxOffice.DAL.Models.Enums;

namespace BoxOffice.BLL.Services.Interfaces
{
    public interface IPosterService
    {
        Task<PagedResponse<GetPosterDto>> GetAllAsync(SearchPosterDto? dto, int page = 1, int itemsPerPage = 10, CancellationToken ct = default);
        Task<GetPosterWithTicketInfosDto> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<GetPosterWithTicketInfosDto> AddAsync(CreatePosterDto createDto, CancellationToken ct = default);
        Task<GetPosterDto> UpdateAsync(Guid id, UpdatePosterDto updateDto, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
        Task ForceDeleteAsync(Guid id, CancellationToken ct = default);
        Task<GetPosterWithTicketsDto> GetPosterTicketsAsync(Guid id, TicketState state, CancellationToken ct = default);
        Task<GetPosterStatsDto> GetPosterStatisticsAsync(Guid id, CancellationToken ct = default);
    }
}
