namespace BoxOffice.BLL.DTO
{
    public class OccupancyReportDto
    {

        public PosterDto Poster { get; set; } = null!;
        public int TotalSeats { get; set; }
        public int AvailableSeats { get; set; }
        public int BookedSeats { get; set; }
        public int SoldSeats { get; set; }
        public decimal OccupancyRate { get; set; }
        public List<TicketTypeStatisticsDto>? TicketTypeStatistics = [];
        public IEnumerable<SeatStatusDto> Seats { get; set; } = [];
    }
}