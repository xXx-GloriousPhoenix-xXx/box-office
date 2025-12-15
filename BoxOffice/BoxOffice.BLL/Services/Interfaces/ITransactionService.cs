using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TransactionDtos;

namespace BoxOffice.BLL.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<PagedResponse<GetTransactionDto>> GetAllAsync(int page, int itemsPerPage);
        Task<GetTransactionDto> GetByIdAsync(Guid id);
        Task<GetTransactionDto> AddAsync(CreateTransactionDto createDto);
        Task DeleteAsync(Guid id);
        Task<PagedResponse<GetTransactionDto>> GetCustomerTransactionsAsync(int page, int itemsPerPage, Guid customerId);
        Task<GetTransactionDto> GetTicketTransactionsAsync(Guid ticketId);
    }
}
