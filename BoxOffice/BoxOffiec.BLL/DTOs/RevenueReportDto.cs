namespace BoxOffice.BLL.DTO
{
    public class RevenueReportDto
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalTicketsSold { get; set; }
        public decimal AverageDailyRevenue { get; set; }
        public decimal AverageTicketPrice { get; set; }

        public IEnumerable<DailyRevenueDto> DailyRevenues { get; set; } = [];
    }
}