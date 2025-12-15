using BoxOffice.DAL.Models.Enums;

namespace BoxOffice.BLL.DTOs.TransactionDtos
{
    public class GetTransactionDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public TransactionType TransactionType { get; set; }
        public Guid CustomerId { get; set; }
        public Guid TicketId { get; set; }
    }
}
