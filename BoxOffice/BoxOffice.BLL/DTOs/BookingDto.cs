using BoxOffice.DAL.Enums;

namespace BoxOffice.BLL.DTO
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public string BookingToken { get; set; } = string.Empty;
        public BookingStatus Status { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime ExpiresAt { get; set; }
        public decimal TotalAmount { get; set; }
        public CustomerDto Customer { get; set; } = null!;
    }
}