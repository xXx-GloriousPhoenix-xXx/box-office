using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.CustomerDtos;

namespace BoxOffice.BLL.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<PagedResponse<GetCustomerDto>> GetAllAsync(int page, int itemsPerPage);
        Task<GetCustomerDto> GetByIdAsync(Guid id);
        Task<GetCustomerDto> AddAsync(CreateCustomerDto createDto);
        Task<GetCustomerDto> UpdateAsync(Guid id, UpdateCustomerDto updateDto);
        Task DeleteAsync(Guid id);
        Task<GetCustomerWithTicketsDto> GetCustomerTicketsAsync(Guid id);
        Task<GetCustomerWithBookingsDto> GetCustomerBookingsAsync(Guid id);

    }
}
