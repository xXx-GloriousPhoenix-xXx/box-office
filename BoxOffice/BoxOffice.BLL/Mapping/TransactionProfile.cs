using AutoMapper;
using BoxOffice.BLL.DTOs.TransactionDtos;
using BoxOffice.DAL.Models.Entities;

namespace BoxOffice.BLL.Mapping
{
    public class TransactionProfile : Profile
    {
        public TransactionProfile()
        {
            CreateMap<CreateTransactionDto, Transaction>()
                .ForMember(dest => dest.Date, opt => opt.Ignore());

            CreateMap<Transaction, GetTransactionDto>();
        }
    }
}
