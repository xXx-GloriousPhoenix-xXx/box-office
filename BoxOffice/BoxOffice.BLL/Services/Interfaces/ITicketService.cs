using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TicketDtos;

namespace BoxOffice.BLL.Services.Interfaces
{
    public interface ITicketService
    {
        Task<PagedResponse<GetTicketDto>> GetAllAsync(int page, int itemsPerPage);
        Task<GetTicketDto> GetByIdAsync(Guid id);
        Task<GetTicketDto> AddAsync(CreateTicketDto createDto);
        Task DeleteAsync(Guid id);
        Task<GetTicketDto> PurchaseAsync(Guid id);
        Task<GetTicketDto> CancelPurchaseAsync(Guid id);
    }
}
