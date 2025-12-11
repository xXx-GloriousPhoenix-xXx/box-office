namespace BoxOffice.BLL.DTO
{
    public class SaleResultDto
    {
        public Guid TicketId { get; set; }
        public Guid TransactionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime SoldDate { get; set; }
    }
}