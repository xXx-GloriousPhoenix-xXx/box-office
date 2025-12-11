using BoxOffice.BLL.DTO;

namespace BoxOffice.BLL.Interfaces
{
    public interface ITicketService
    {
        Task<IEnumerable<TicketDto>> GetAvailableTicketsAsync(Guid posterId, CancellationToken ct = default);
        Task<TicketDetailsDto?> GetTicketDetailsAsync(Guid ticketId, CancellationToken ct = default);
        Task<BookingResultDto> BookTicketsAsync(BookTicketsDto bookDto, CancellationToken ct = default);
        Task<SaleResultDto> SellTicketAsync(SellTicketDto sellDto, CancellationToken ct = default);
        Task<bool> CancelBookingAsync(string bookingToken, CancellationToken ct = default);
        Task<bool> ConvertBookingToSaleAsync(string bookingToken, CancellationToken ct = default);
        Task<IEnumerable<TicketDto>> GetCustomerTicketsAsync(Guid customerId, CancellationToken ct = default);
    }
}