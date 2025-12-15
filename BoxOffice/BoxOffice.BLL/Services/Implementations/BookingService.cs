using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.BookingDtos;
using BoxOffice.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoxOffice.BLL.Services.Implementations
{
    public class BookingService : IBookingService
    {
        public Task<GetBookingDto> AddAsync(CreateBookingDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task<GetBookingDto> BookAsync(Guid ticketId)
        {
            throw new NotImplementedException();
        }

        public Task CancelBookingAsync(Guid ticketId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<GetBookingDto>> GetAllAsync(int page, int itemsPerPage)
        {
            throw new NotImplementedException();
        }

        public Task<GetBookingDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
