namespace BoxOffice.BLL.DTOs.TicketDtos
{
    public class CreateTicketDto
    {
        public required Guid TicketInfoId { get; set; }
        public required string SeatNumber { get; set; }
    }
}
