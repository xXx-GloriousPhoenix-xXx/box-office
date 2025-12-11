namespace BoxOffice.BLL.DTO
{
    public class BookingResultDto
    {
        public Guid BookingId { get; set; }
        public string BookingToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public decimal TotalAmount { get; set; }
        public List<TicketDto> Tickets { get; set; } = new();
    }
}