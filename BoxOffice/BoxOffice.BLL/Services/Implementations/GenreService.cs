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
        public Task<GetGenreDto> AddAsync(CreateGenreDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedResponse<GetGenreDto>> GetAllAsync(int page, int itemsPerPage)
        {
            throw new NotImplementedException();
        }

        public Task<GetGenreDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GetGenreDto> UpdateAsync(Guid id, UpdateGenreDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
