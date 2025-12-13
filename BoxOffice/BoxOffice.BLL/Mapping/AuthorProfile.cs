using AutoMapper;
using BoxOffice.BLL.DTO;
using BoxOffice.BLL.DTOs;
using BoxOffice.DAL.Entities;

namespace BoxOffice.BLL.Mapping
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, AuthorDto>();

            CreateMap<CreateAuthorDto, Author>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Posters, opt => opt.Ignore());

            CreateMap<UpdateAuthorDto, Author>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}