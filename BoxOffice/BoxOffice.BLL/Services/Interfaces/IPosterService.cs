using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.PosterDtos;
using BoxOffice.DAL.Models.Enums;

namespace BoxOffice.BLL.Services.Interfaces
{
    public interface IPosterService
    {
        Task<PagedResponse<GetPosterWithTicketInfosDto>> GetAllAsync(int page, int itemsPerPage, SearchPosterDto? dto);
        Task<GetPosterWithTicketInfosDto> GetByIdAsync(Guid id);
        Task<GetPosterWithTicketInfosDto> AddAsync(CreatePosterDto createDto);
        Task<GetPosterDto> UpdateAsync(Guid id, UpdatePosterDto updateDto);
        Task DeleteAsync(Guid id);
        Task<GetPosterWithTicketsDto> GetPosterTicketsAsync(Guid id, TicketState state);
        Task<GetPosterStatsDto> GetPosterStatisticsAsync(Guid id);
    }
}
