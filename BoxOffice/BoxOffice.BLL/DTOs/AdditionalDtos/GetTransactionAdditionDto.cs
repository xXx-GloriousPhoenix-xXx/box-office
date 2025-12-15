using BoxOffice.DAL.Models.Enums;

namespace BoxOffice.BLL.DTOs.AdditionalDtos
{
    public class GetTransactionAdditionDto
    {
        public decimal Amount { get; set; }
        public DateOnly Date { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public TransactionType TransactionType { get; set; }
    }
}
