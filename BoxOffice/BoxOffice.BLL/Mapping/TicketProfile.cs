using AutoMapper;
using BoxOffice.BLL.DTO;
using BoxOffice.DAL.Entities;
using BoxOffice.DAL.Enums;

namespace BoxOffice.BLL.Mapping
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<Ticket, TicketDto>()
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.TicketInfo != null ? src.TicketInfo.Price : 0))
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.TicketInfo != null ? src.TicketInfo.TicketType : TicketType.Standard))
                .ForMember(dest => dest.State,
                    opt => opt.MapFrom(src => src.State));

            CreateMap<Ticket, TicketDetailsDto>()
                .IncludeBase<Ticket, TicketDto>()
                .ForMember(dest => dest.Poster,
                    opt => opt.MapFrom(src => src.Poster))
                .ForMember(dest => dest.Customer,
                    opt => opt.MapFrom(src => src.Customer))
                .ForMember(dest => dest.Booking,
                    opt => opt.MapFrom(src => src.Booking));

            CreateMap<BookTicketsDto, Booking>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.BookingToken, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.BookingDate, opt => opt.Ignore())
                .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore())
                .ForMember(dest => dest.Tickets, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId,
                    opt => opt.MapFrom(src => src.CustomerId));
        }
    }
}