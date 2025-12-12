using AutoMapper;
using BoxOffice.BLL.DTO;
using BoxOffice.DAL.Entities;

namespace BoxOffice.BLL.Mapping
{
    public class PosterProfile : Profile
    {
        public PosterProfile()
        {
            CreateMap<Poster, PosterDto>()
                .ForMember(dest => dest.Author,
                    opt => opt.MapFrom(src => src.Author))
                .ForMember(dest => dest.TicketTypes,
                    opt => opt.MapFrom(src => src.TicketInfos))
                .ForMember(dest => dest.Genres,
                    opt => opt.MapFrom(src => src.Genres));

            CreateMap<Poster, PosterDetailsDto>()
                .IncludeBase<Poster, PosterDto>();

            CreateMap<CreatePosterDto, Poster>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Author,
                    opt => opt.Ignore())
                .ForMember(dest => dest.AuthorId,
                    opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.TicketInfos,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Tickets,
                    opt => opt.Ignore());

            CreateMap<UpdatePosterDto, Poster>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) =>
                        srcMember != null));
        }
    }
}