using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.AdditionalDtos.OperationDtos;
using BoxOffice.BLL.DTOs.BookingDtos;

namespace BoxOffice.BLL.Services.Interfaces
{
    public interface IBookingService
    {
        Task<PagedResponse<GetBookingDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default);
        Task<GetBookingDto> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<GetBookingDto> BookAsync(Guid ticketId, BookingDto dto, CancellationToken ct = default);
        Task CancelBookingAsync(Guid ticketId, CancelBookingDto dto, CancellationToken ct = default);
    }
}
