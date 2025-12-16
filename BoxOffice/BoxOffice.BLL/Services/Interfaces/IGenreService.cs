using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.GenreDtos;

namespace BoxOffice.BLL.Services.Interfaces
{
    public interface IGenreService
    {
        Task<PagedResponse<GetGenreDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default);
        Task<GetGenreDto> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<GetGenreDto> AddAsync(CreateGenreDto createDto, CancellationToken ct = default);
        Task<GetGenreDto> UpdateAsync(Guid id, UpdateGenreDto updateDto, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
