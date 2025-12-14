using BoxOffice.BLL.DTOs.AuthorDtos;
using BoxOffice.BLL.Interfaces;

namespace BoxOffice.BLL.Services
{
    public class AuthorService : IAuthorService
    {
        public Task<GetAuthorDto> AddAsync(CreateAuthorDto createDto)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GetAuthorDto>> GetAllAsync(int page, int itemsPerPage)
        {
            throw new NotImplementedException();
        }

        public Task<GetAuthorWithPostersDto> GetAuthorPostersAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GetAuthorDto> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<GetAuthorDto> UpdateAsync(Guid id, UpdateAuthorDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
