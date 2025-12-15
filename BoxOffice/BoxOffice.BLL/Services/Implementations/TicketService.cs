using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TicketDtos;
using BoxOffice.BLL.Services.Interfaces;

namespace BoxOffice.BLL.Services.Implementations
{
    public class TicketService : ITicketService
    {
        public Task<GetTicketDto> AddAsync(CreateTicketDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<GetTicketDto> CancelPurchaseAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<GetTicketDto>> GetAllAsync(int page, int itemsPerPage)
        {
            throw new NotImplementedException();
        }

        public Task<GetTicketDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GetTicketDto> PurchaseAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
