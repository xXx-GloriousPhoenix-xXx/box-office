using AutoMapper;
using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.CustomerDtos;
using BoxOffice.DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoxOffice.BLL.Mapping
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {
            CreateMap<CreateCustomerDto, Customer>()
            .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber));

            CreateMap<Customer, GetCustomerDto>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Phone));

            CreateMap<Customer, GetCustomerWithTicketsDto>()
                .IncludeBase<Customer, GetCustomerDto>();

            CreateMap<Booking, GetBookingAdditionDto>();
            CreateMap<Ticket, GetTicketAdditionDto>();
        }
    }
}
