using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.BookingDtos;

namespace BoxOffice.BLL.Services.Interfaces
{
    public interface IBookingService
    {
        Task<PagedResponse<GetBookingDto>> GetAllAsync(int page, int itemsPerPage);
        Task<GetBookingDto> GetByIdAsync(Guid id);
        Task<GetBookingDto> AddAsync(CreateBookingDto createDto);
        Task DeleteAsync(Guid id);
        Task<GetBookingDto> BookAsync(Guid ticketId);
        Task CancelBookingAsync(Guid ticketId);
    }
}
