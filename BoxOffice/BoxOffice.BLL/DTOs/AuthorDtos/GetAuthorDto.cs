namespace BoxOffice.BLL.DTOs.AuthorDtos
{
    public class GetAuthorDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
    }
}
