using BoxOffice.BLL.DTOs.AuthorDtos;
using BoxOffice.BLL.DTOs.AdditionalDtos;

namespace BoxOffice.BLL.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<PagedResponse<GetAuthorDto>> GetAllAsync(int page, int itemsPerPage, CancellationToken ct = default);
        Task<GetAuthorDto> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<GetAuthorDto> AddAsync(CreateAuthorDto createDto, CancellationToken ct = default);
        Task<GetAuthorDto> UpdateAsync(Guid id, UpdateAuthorDto updateDto, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
        Task<GetAuthorWithPostersDto> GetAuthorPostersAsync(Guid id, CancellationToken ct = default);
    }
}
