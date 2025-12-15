using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TicketInfoDtos;

namespace BoxOffice.BLL.Services.Interfaces
{
    public interface ITicketInfoService
    {
        Task<PagedResponse<GetTicketInfoDto>> GetAllAsync(int page, int itemsPerPage);
        Task<GetTicketInfoDto> GetByIdAsync(Guid id);
        Task<GetTicketInfoDto> AddAsync(CreateTicketInfoDto createDto);
        Task<GetTicketInfoDto> UpdateAsync(Guid id, UpdateTicketInfoDto updateDto);
        Task DeleteAsync(Guid id);
    }
}
