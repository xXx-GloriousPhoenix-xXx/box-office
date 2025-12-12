using AutoMapper;
using BoxOffice.BLL.DTO;
using BoxOffice.DAL.Entities;

namespace BoxOffice.BLL.Mapping
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>();

            CreateMap<AuthorDto, Author>()
                .ForMember(dest => dest.Posters,
                    opt => opt.Ignore());
        }
    }
}