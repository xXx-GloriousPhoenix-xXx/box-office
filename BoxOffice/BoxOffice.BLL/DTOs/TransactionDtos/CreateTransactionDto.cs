using BoxOffice.DAL.Models.Enums;

namespace BoxOffice.BLL.DTOs.TransactionDtos
{
    public class CreateTransactionDto
    {
        public required decimal Amount { get; set; }
        public required PaymentMethod PaymentMethod { get; set; }
        public required TransactionType TransactionType { get; set; }
        public required Guid CustomerId { get; set; }
        public required Guid TicketId { get; set; }
    }
}
