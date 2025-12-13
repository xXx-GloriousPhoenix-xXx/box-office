namespace BoxOffice.BLL.DTO
{
    public class CustomerDetailsDto : CustomerDto
    {
        public string? PhoneNumber { get; set; }
        public int TotalPurchases { get; set; }
        public decimal TotalSpent { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public IEnumerable<BookingDto> ActiveBookings { get; set; } = [];
    }
}