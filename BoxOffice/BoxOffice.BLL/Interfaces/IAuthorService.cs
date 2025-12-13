using BoxOffice.BLL.DTO;
using BoxOffice.BLL.DTOs;

namespace BoxOffice.BLL.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync(CancellationToken ct = default);
        Task<AuthorDto?> GetAuthorByIdAsync(Guid id, CancellationToken ct = default);
        Task<AuthorDto> CreateAuthorAsync(CreateAuthorDto createDto, CancellationToken ct = default);
        Task<AuthorDto> UpdateAuthorAsync(Guid id, UpdateAuthorDto updateDto, CancellationToken ct = default);
        Task<bool> DeleteAuthorAsync(Guid id, CancellationToken ct = default);
    }
}
