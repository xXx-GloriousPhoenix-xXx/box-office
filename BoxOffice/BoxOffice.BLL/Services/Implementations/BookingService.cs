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
        public Task<GetBookingDto> AddAsync(CreateBookingDto createDto, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<GetBookingDto> BookAsync(Guid ticketId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task CancelBookingAsync(Guid ticketId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<GetBookingDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<GetBookingDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
