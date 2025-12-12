using AutoMapper;
using BoxOffice.BLL.DTO;
using BoxOffice.DAL.Entities;

namespace BoxOffice.BLL.Mapping
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<Booking, BookingDto>()
                .ForMember(dest => dest.Customer,
                    opt => opt.MapFrom(src => src.Customer));

            CreateMap<Booking, BookingDetailsDto>()
                .IncludeBase<Booking, BookingDto>()
                .ForMember(dest => dest.Tickets,
                    opt => opt.MapFrom(src => src.Tickets));

            CreateMap<Booking, BookingResultDto>()
                .ForMember(dest => dest.BookingId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Tickets,
                    opt => opt.MapFrom(src => src.Tickets));
        }
    }
}