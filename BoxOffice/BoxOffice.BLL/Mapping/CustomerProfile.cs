using AutoMapper;
using BoxOffice.BLL.DTO;
using BoxOffice.DAL.Entities;

namespace BoxOffice.BLL.Mapping
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<Customer, CustomerDto>();

            CreateMap<BookTicketsDto, Customer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PurchasedTickets, opt => opt.Ignore())
                .ForMember(dest => dest.Bookings, opt => opt.Ignore())
                .ForMember(dest => dest.Transactions, opt => opt.Ignore())
                .ForMember(dest => dest.PhoneNumber, opt => opt.Ignore());
        }
    }
}