using AutoMapper;
using BoxOffice.BLL.DTOs.BookingDtos;
using BoxOffice.DAL.Models.Entities;

namespace BoxOffice.BLL.Mapping
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<CreateBookingDto, Booking>()
            .ForMember(dest => dest.BookedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore())
            .ForMember(dest => dest.BookingToken, opt => opt.Ignore())
            .ForMember(dest => dest.State, opt => opt.Ignore());

            CreateMap<Booking, GetBookingDto>();
        }
    }
}
