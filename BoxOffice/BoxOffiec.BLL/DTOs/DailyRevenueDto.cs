namespace BoxOffice.BLL.DTO
{
    public class DailyRevenueDto
    {
        public DateOnly Date { get; set; }
        public decimal Revenue { get; set; }
        public int TicketsSold { get; set; }
    }
}