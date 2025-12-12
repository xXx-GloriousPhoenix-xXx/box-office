namespace BoxOffice.BLL.DTO
{
    public class BookingDetailsDto : BookingDto
    {
        public List<TicketDto> Tickets { get; set; } = new();
        public List<TransactionDto> Transactions { get; set; } = new();
    }
}