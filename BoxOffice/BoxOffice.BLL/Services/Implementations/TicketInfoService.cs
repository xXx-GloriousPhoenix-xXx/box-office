using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TicketInfoDtos;
using BoxOffice.BLL.Services.Interfaces;

namespace BoxOffice.BLL.Services.Implementations
{
    public class TicketInfoService : ITicketInfoService
    {
        public Task<GetTicketInfoDto> AddAsync(CreateTicketInfoDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<GetTicketInfoDto>> GetAllAsync(int page, int itemsPerPage)
        {
            throw new NotImplementedException();
        }

        public Task<GetTicketInfoDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GetTicketInfoDto> UpdateAsync(Guid id, UpdateTicketInfoDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
