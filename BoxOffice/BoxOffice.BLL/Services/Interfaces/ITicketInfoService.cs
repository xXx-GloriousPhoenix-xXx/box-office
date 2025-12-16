using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TicketInfoDtos;

namespace BoxOffice.BLL.Services.Interfaces
{
    public interface ITicketInfoService
    {
        Task<PagedResponse<GetTicketInfoDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default);
        Task<GetTicketInfoDto> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<GetTicketInfoDto> AddAsync(CreateTicketInfoDto createDto, CancellationToken ct = default);
        Task<GetTicketInfoDto> UpdateAsync(Guid id, UpdateTicketInfoDto updateDto, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
