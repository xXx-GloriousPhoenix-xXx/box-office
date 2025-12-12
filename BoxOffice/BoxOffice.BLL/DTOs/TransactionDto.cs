using BoxOffice.DAL.Enums;

namespace BoxOffice.BLL.DTO
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}