using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.GenreDtos;
using BoxOffice.BLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoxOffice.BLL.Services.Implementations
{
    public class GenreService : IGenreService
    {
        public Task<GetGenreDto> AddAsync(CreateGenreDto createDto, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<GetGenreDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<GetGenreDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<GetGenreDto> UpdateAsync(Guid id, UpdateGenreDto updateDto, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
