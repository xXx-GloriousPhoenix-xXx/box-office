using BoxOffice.BLL.DTOs.AuthorDtos;

namespace BoxOffice.BLL.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<GetAuthorDto>> GetAllAsync(int page, int itemsPerPage);
        Task<GetAuthorDto> GetByIdAsync(Guid id);
        Task<GetAuthorDto> AddAsync(CreateAuthorDto createDto);
        Task<GetAuthorDto> UpdateAsync(Guid id, UpdateAuthorDto updateDto);
        Task DeleteAsync(Guid id);
        Task<GetAuthorWithPostersDto> GetAuthorPostersAsync(Guid id);
    }
}
