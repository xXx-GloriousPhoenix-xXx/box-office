namespace BoxOffice.BLL.DTO
{
    public class SalesTrendDataPointDto
    {
        public string Period { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int TicketsSold { get; set; }
        public decimal AverageTicketPrice { get; set; }
    }
}