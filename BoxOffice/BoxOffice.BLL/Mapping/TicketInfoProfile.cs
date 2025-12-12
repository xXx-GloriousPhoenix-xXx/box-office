using AutoMapper;
using BoxOffice.BLL.DTO;
using BoxOffice.DAL.Entities;

namespace BoxOffice.BLL.Mapping
{
    public class TicketInfoProfile : Profile
    {
        public TicketInfoProfile()
        {
            CreateMap<TicketInfo, TicketTypeInfoDto>()
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.TicketType));

            CreateMap<CreateTicketTypeInfoDto, TicketInfo>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PosterId, opt => opt.Ignore())
                .ForMember(dest => dest.Poster, opt => opt.Ignore())
                .ForMember(dest => dest.Tickets, opt => opt.Ignore())
                .ForMember(dest => dest.AvailableTickets,
                    opt => opt.MapFrom(src => src.TotalTickets))
                .ForMember(dest => dest.SoldTickets,
                    opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.BookedTickets,
                    opt => opt.MapFrom(src => 0));
        }
    }
}