using BoxOffice.DAL.Enums;

namespace BoxOffice.BLL.DTO
{
    public class TicketTypeStatisticsDto
    {
        public TicketType TicketType { get; set; }
        public int Total { get; set; }
        public int Available { get; set; }
        public int Booked { get; set; }
        public int Sold { get; set; }
        public decimal Revenue { get; set; }
        public decimal OccupancyRate => Total > 0 ? (decimal)(Booked + Sold) / Total * 100 : 0;
    }
}