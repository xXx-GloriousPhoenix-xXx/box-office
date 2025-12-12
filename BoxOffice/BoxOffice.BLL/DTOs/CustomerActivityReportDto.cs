namespace BoxOffice.BLL.DTO
{
    public class CustomerActivityReportDto
    {
        public CustomerDto Customer { get; set; } = null!;
        public decimal TotalSpent { get; set; }
        public int TicketsPurchased { get; set; }
        public int ActiveBookings { get; set; }
        public int CompletedBookings { get; set; }
        public DateTime? FirstPurchaseDate { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public decimal AverageTicketPrice { get; set; }
        public List<GenrePreferenceDto> GenrePreferences { get; set; } = new();
    }
}