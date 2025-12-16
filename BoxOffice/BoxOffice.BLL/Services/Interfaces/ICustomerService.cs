using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.CustomerDtos;

namespace BoxOffice.BLL.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<PagedResponse<GetCustomerDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default);
        Task<GetCustomerDto> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<GetCustomerDto> AddAsync(CreateCustomerDto createDto, CancellationToken ct = default);
        Task<GetCustomerDto> UpdateAsync(Guid id, UpdateCustomerDto updateDto, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
        Task<GetCustomerWithTicketsDto> GetCustomerTicketsAsync(Guid id, CancellationToken ct = default);
        Task<GetCustomerWithBookingsDto> GetCustomerBookingsAsync(Guid id, CancellationToken ct = default);
    }
}
