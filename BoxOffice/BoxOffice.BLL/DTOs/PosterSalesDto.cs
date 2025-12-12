namespace BoxOffice.BLL.DTO
{
    public class PosterSalesDto
    {
        public PosterDto Poster { get; set; } = null!;
        public int TicketsSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal OccupancyRate { get; set; }
    }
}