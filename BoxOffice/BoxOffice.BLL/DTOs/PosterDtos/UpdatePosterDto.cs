namespace BoxOffice.BLL.DTOs.PosterDtos
{
    public class UpdatePosterDto
    {
        public Guid? AuthorId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Venue { get; set; }
        public DateOnly? Date { get; set; }
        public int? Duration { get; set; }
        public ICollection<string>? Genres { get; set; }
    }
}
