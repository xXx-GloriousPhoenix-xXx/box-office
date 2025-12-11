namespace BoxOffice.BLL.DTO
{
    public class SalesTrendReportDto
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string Interval { get; set; } = string.Empty;
        public List<SalesTrendDataPointDto> DataPoints { get; set; } = new();
        public decimal TotalRevenue { get; set; }
        public int TotalTicketsSold { get; set; }
        public decimal PeakRevenue { get; set; }
        public int PeakTicketsSold { get; set; }
        public decimal AverageDailyRevenue => DataPoints.Any() ?
            DataPoints.Average(d => d.Revenue) : 0;
    }
}