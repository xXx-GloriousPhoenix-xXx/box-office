using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TransactionDtos;
using BoxOffice.BLL.Services.Interfaces;

namespace BoxOffice.BLL.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        public Task<GetTransactionDto> AddAsync(CreateTransactionDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<GetTransactionDto>> GetAllAsync(int page, int itemsPerPage)
        {
            throw new NotImplementedException();
        }

        public Task<GetTransactionDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<GetTransactionDto>> GetCustomerTransactionsAsync(int page, int itemsPerPage, Guid customerId)
        {
            throw new NotImplementedException();
        }

        public Task<GetTransactionDto> GetTicketTransactionsAsync(Guid ticketId)
        {
            throw new NotImplementedException();
        }
    }
}
