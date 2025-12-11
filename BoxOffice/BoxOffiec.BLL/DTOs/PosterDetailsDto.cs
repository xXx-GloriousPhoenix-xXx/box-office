namespace BoxOffice.BLL.DTO
{
    public class PosterDetailsDto : PosterDto
    {
        public int TotalTickets { get; set; }
        public int AvailableTickets { get; set; }
        public int SoldTickets { get; set; }
        public int BookedTickets { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}