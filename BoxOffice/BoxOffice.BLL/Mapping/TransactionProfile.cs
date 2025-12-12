using AutoMapper;
using BoxOffice.BLL.DTO;
using BoxOffice.DAL.Entities;
using BoxOffice.DAL.Enums;

namespace BoxOffice.BLL.Mapping
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<Transaction, TransactionDto>();

            CreateMap<SellTicketDto, Transaction>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionDate, opt => opt.Ignore())
                .ForMember(dest => dest.TransactionType,
                    opt => opt.MapFrom(_ => TransactionType.Purchase))
                .ForMember(dest => dest.Amount, opt => opt.Ignore())
                .ForMember(dest => dest.Ticket, opt => opt.Ignore())
                .ForMember(dest => dest.Customer, opt => opt.Ignore());

            CreateMap<Transaction, SaleResultDto>()
                .ForMember(dest => dest.TicketId,
                    opt => opt.MapFrom(src => src.TicketId))
                .ForMember(dest => dest.TransactionId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SoldDate,
                    opt => opt.MapFrom(src => src.TransactionDate));
        }
    }
}