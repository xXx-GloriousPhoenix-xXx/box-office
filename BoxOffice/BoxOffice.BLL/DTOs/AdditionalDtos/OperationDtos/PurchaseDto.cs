using BoxOffice.DAL.Models.Enums;

namespace BoxOffice.BLL.DTOs.AdditionalDtos.OperationDtos
{
    public class PurchaseDto
    {
        public required PaymentMethod PaymentMethod { get; set; }
        public required Guid CustomerId { get; set; }
    }
}
