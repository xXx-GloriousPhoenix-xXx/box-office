using AutoMapper;
using BoxOffice.BLL.DTOs.TicketInfoDtos;
using BoxOffice.DAL.Models.Entities;

namespace BoxOffice.BLL.Mapping
{
    public class TicketInfoProfile : Profile
    {
        public TicketInfoProfile()
        {
            CreateMap<CreateTicketInfoDto, TicketInfo>()
                .ForMember(dest => dest.AvailableCount, opt => opt.MapFrom(src => src.TotalCount))
                .ForMember(dest => dest.SoldCount, opt => opt.Ignore())
                .ForMember(dest => dest.BookedCount, opt => opt.Ignore());

            CreateMap<TicketInfo, GetTicketInfoDto>();
        }
    }
}
