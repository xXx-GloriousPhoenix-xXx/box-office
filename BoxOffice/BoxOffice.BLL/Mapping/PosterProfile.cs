using AutoMapper;
using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.PosterDtos;
using BoxOffice.DAL.Models.Entities;

namespace BoxOffice.BLL.Mapping
{
    public class PosterProfile : Profile
    {
        public PosterProfile()
        {
            CreateMap<CreatePosterDto, Poster>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.Genres, opt => opt.Ignore())
                .ForMember(dest => dest.TicketInfos, opt => opt.Ignore());

            CreateMap<Poster, GetPosterDto>()
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author!.Name))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.Genres.Select(g => g.Name)));

            CreateMap<Poster, GetPosterWithTicketInfosDto>()
            .IncludeBase<Poster, GetPosterDto>()
            .ForMember(dest => dest.TicketInfos, opt => opt.Ignore());

            CreateMap<Poster, GetPosterWithTicketsDto>()
                .IncludeBase<Poster, GetPosterDto>()
                .ForMember(dest => dest.Tickets, opt => opt.Ignore());

            CreateMap<TicketInfo, GetTicketInfoAdditionDto>();

            CreateMap<Ticket, GetTicketAdditionDto>();

            CreateMap<Poster, GetPosterStatsDto>()
                .IncludeBase<Poster, GetPosterDto>();
        }
    }
}
