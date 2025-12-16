using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TicketInfoDtos;
using BoxOffice.BLL.Services.Interfaces;

namespace BoxOffice.BLL.Services.Implementations
{
    public class TicketInfoService : ITicketInfoService
    {
        public Task<GetTicketInfoDto> AddAsync(CreateTicketInfoDto createDto, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<GetTicketInfoDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<GetTicketInfoDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<GetTicketInfoDto> UpdateAsync(Guid id, UpdateTicketInfoDto updateDto, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
