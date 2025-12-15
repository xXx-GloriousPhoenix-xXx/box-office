namespace BoxOffice.BLL.DTOs.PosterDtos
{
    public class GetPosterDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Author { get; set; }
        public required string Description { get; set; }
        public required string Venue { get; set; }
        public DateOnly Date { get; set; }
        public int Duration { get; set; }
        public required ICollection<string> Genres { get; set; }
    }
}
