using AutoMapper;
using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TicketDtos;
using BoxOffice.DAL.Models.Entities;

namespace BoxOffice.BLL.Mapping
{
    public class TicketProfile : Profile
    {
        public TicketProfile()
        {
            CreateMap<CreateTicketDto, Ticket>()
                .ForMember(dest => dest.TicketState, opt => opt.Ignore());

            CreateMap<Ticket, GetTicketDto>()
                .ForMember(dest => dest.TicketState, opt => opt.MapFrom(src => src.TicketState))
                .ForMember(dest => dest.BookingId, opt => opt.MapFrom(src => src.BookingId));

            CreateMap<Ticket, GetTicketWithTransactionsDto>()
                .IncludeBase<Ticket, GetTicketDto>();

            CreateMap<Transaction, GetTransactionAdditionDto>();
        }
    }
}
