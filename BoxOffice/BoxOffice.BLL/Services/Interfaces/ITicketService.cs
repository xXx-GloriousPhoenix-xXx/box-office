using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TicketDtos;

namespace BoxOffice.BLL.Services.Interfaces
{
    public interface ITicketService
    {
        Task<PagedResponse<GetTicketDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default);
        Task<GetTicketDto> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<GetTicketDto> AddAsync(CreateTicketDto createDto, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
        Task<GetTicketDto> PurchaseAsync(Guid id, CancellationToken ct = default);
        Task<GetTicketDto> CancelPurchaseAsync(Guid id, CancellationToken ct = default);
    }
}
