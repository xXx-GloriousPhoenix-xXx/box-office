using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.PosterDtos;
using BoxOffice.BLL.Services.Interfaces;
using BoxOffice.DAL.Models.Enums;

namespace BoxOffice.BLL.Services.Implementations
{
    public class PosterService : IPosterService
    {
        public Task<GetPosterWithTicketInfosDto> AddAsync(CreatePosterDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<GetPosterWithTicketInfosDto>> GetAllAsync(int page, int itemsPerPage, SearchPosterDto? dto)
        {
            throw new NotImplementedException();
        }

        public Task<GetPosterWithTicketInfosDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GetPosterStatsDto> GetPosterStatisticsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GetPosterWithTicketsDto> GetPosterTicketsAsync(Guid id, TicketState state)
        {
            throw new NotImplementedException();
        }

        public Task<GetPosterDto> UpdateAsync(Guid id, UpdatePosterDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
