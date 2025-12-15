using AutoMapper;
using BoxOffice.BLL.DTOs.AuthorDtos;
using BoxOffice.DAL.Models.Entities;

namespace BoxOffice.BLL.Mapping
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<Author, GetAuthorDto>();

            CreateMap<CreateAuthorDto, Author>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Posters,
                    opt => opt.Ignore());

            CreateMap<UpdateAuthorDto, Author>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) =>
                        srcMember != null));

            CreateMap<Author, GetAuthorWithPostersDto>()
                .IncludeBase<Author, GetAuthorDto>()
                .ForMember(dest => dest.Posters,
                    opt => opt.MapFrom(src => src.Posters));
        }
    }
}
