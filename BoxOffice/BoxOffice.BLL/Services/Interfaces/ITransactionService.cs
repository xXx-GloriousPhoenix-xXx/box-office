using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TransactionDtos;

namespace BoxOffice.BLL.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<PagedResponse<GetTransactionDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default);
        Task<GetTransactionDto> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<GetTransactionDto> AddAsync(CreateTransactionDto createDto, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
        Task<PagedResponse<GetTransactionDto>> GetCustomerTransactionsAsync(Guid customerId, int page = 1, int itemsPerPage = 10, CancellationToken ct = default);
        Task<GetTransactionDto> GetTicketTransactionsAsync(Guid ticketId, CancellationToken ct = default);
    }
}
