using BoxOffice.DAL.Enums;

namespace BoxOffice.BLL.DTO
{
    public class GenrePreferenceDto
    {
        public PosterGenre Genre { get; set; }
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }
}