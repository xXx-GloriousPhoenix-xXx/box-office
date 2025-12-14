using BoxOffice.BLL.DTOs.CustomerDtos;

namespace BoxOffice.BLL.Interfaces
{
    public interface ICustomerService
    {
        Task<IEnumerable<GetCustomerDto>> GetAllAsync(int page, int itemsPerPage);
        Task<GetCustomerDto> GetByIdAsync(Guid id);
        Task<GetCustomerDto> AddAsync(CreateCustomerDto createDto);
        Task<GetCustomerDto> UpdateAsync(Guid id, UpdateCustomerDto updateDto);
        Task DeleteAsync(Guid id);
        Task<GetCustomerWithTicketsDto> GetCustomerTicketsAsync(Guid id);
        Task<GetCustomerWithBookingsDto> GetCustomerBookingsAsync(Guid id);

    }
    public interface IGenreService
    {

    }
    public interface IPosterService
    {

    }
    public interface ITicketInfoService
    {

    }
    public interface ITicketService
    {

    }
    public interface IBookingService
    {

    }
    public interface ITransactionService
    {

    }
}
