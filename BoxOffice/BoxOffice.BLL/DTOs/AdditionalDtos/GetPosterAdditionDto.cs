namespace BoxOffice.BLL.DTOs.AdditionalDtos
{
    public class GetPosterAdditionDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Venue { get; set; }
        public DateOnly Date { get; set; }
        public int Duration { get; set; }
    }
}
