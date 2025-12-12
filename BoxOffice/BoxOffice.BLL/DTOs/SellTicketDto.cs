namespace BoxOffice.BLL.DTO
{
    public class SellTicketDto
    {
        public Guid TicketId { get; set; }
        public Guid CustomerId { get; set; }
        public string PaymentMethod { get; set; } = "Card";
        public string? PaymentReference { get; set; }
    }
}