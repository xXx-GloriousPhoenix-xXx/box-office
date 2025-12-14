using BoxOffice.BLL.DTOs.CustomerDtos;
using BoxOffice.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoxOffice.BLL.Services
{
    public class CustomerService : ICustomerService
    {
        public Task<GetCustomerDto> AddAsync(CreateCustomerDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetCustomerDto>> GetAllAsync(int page, int itemsPerPage)
        {
            throw new NotImplementedException();
        }

        public Task<GetCustomerDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GetCustomerWithBookingsDto> GetCustomerBookingsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GetCustomerWithTicketsDto> GetCustomerTicketsAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GetCustomerDto> UpdateAsync(Guid id, UpdateCustomerDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
