namespace BoxOffice.BLL.DTOs.PosterDtos
{
    public class GetPosterStatsDto : GetPosterDto
    {
        public int CountTotal { get; set; }
        public int CountSold { get; set; }
        public int CountBooked { get; set; }
        public int CountAvailable { get; set; }
        public decimal Revenue { get; set; }
        public DateTime ReportTime { get; set; } = DateTime.UtcNow;
    }
}
