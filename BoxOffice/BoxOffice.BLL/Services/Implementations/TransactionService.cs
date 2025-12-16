using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TransactionDtos;
using BoxOffice.BLL.Services.Interfaces;

namespace BoxOffice.BLL.Services.Implementations
{
    public class TransactionService : ITransactionService
    {
        public Task<GetTransactionDto> AddAsync(CreateTransactionDto createDto, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<GetTransactionDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<GetTransactionDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<GetTransactionDto>> GetCustomerTransactionsAsync(Guid customerId, int page = 1, int itemsPerPage = 10, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<GetTransactionDto> GetTicketTransactionsAsync(Guid ticketId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
