using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.PosterDtos;
using BoxOffice.BLL.Services.Interfaces;
using BoxOffice.DAL.Models.Enums;

namespace BoxOffice.BLL.Services.Implementations
{
    public class PosterService : IPosterService
    {
        public Task<GetPosterWithTicketInfosDto> AddAsync(CreatePosterDto createDto, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<GetPosterWithTicketInfosDto>> GetAllAsync(SearchPosterDto? dto, int page = 1, int itemsPerPage = 10, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<GetPosterWithTicketInfosDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<GetPosterStatsDto> GetPosterStatisticsAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<GetPosterWithTicketsDto> GetPosterTicketsAsync(Guid id, TicketState state, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<GetPosterDto> UpdateAsync(Guid id, UpdatePosterDto updateDto, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
