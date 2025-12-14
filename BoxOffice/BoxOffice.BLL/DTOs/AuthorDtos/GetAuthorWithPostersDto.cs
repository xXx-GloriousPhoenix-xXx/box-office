using BoxOffice.BLL.DTOs.AdditionalDtos;

namespace BoxOffice.BLL.DTOs.AuthorDtos
{
    public class GetAuthorWithPostersDto : GetAuthorDto
    {
        public ICollection<GetPosterAdditionDto> Posters { get; set; } = [];
    }
}
