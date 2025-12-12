using AutoMapper;
using BoxOffice.BLL.DTO;
using BoxOffice.DAL.Entities;

namespace BoxOffice.BLL.Mapping
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<Poster, PosterSalesDto>()
                .ForMember(dest => dest.Poster,
                    opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.TicketsSold,
                    opt => opt.Ignore())
                .ForMember(dest => dest.TotalRevenue,
                    opt => opt.Ignore())
                .ForMember(dest => dest.OccupancyRate,
                    opt => opt.Ignore());

            CreateMap<Poster, OccupancyReportDto>()
                .ForMember(dest => dest.Poster,
                    opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.TotalSeats,
                    opt => opt.Ignore())
                .ForMember(dest => dest.AvailableSeats,
                    opt => opt.Ignore())
                .ForMember(dest => dest.BookedSeats,
                    opt => opt.Ignore())
                .ForMember(dest => dest.SoldSeats,
                    opt => opt.Ignore())
                .ForMember(dest => dest.OccupancyRate,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Seats,
                    opt => opt.Ignore())
                .ForMember(dest => dest.TicketTypeStatistics,
                    opt => opt.Ignore());

            CreateMap<Transaction, DailyRevenueDto>()
                .ForMember(dest => dest.Date,
                    opt => opt.MapFrom(src => DateOnly.FromDateTime(src.TransactionDate)))
                .ForMember(dest => dest.Revenue,
                    opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.TicketsSold,
                    opt => opt.MapFrom(_ => 1));
        }
    }
}