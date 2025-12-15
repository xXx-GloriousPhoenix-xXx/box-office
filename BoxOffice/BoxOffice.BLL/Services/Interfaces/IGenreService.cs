using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.GenreDtos;

namespace BoxOffice.BLL.Services.Interfaces
{
    public interface IGenreService
    {
        Task<PagedResponse<GetGenreDto>> GetAllAsync(int page, int itemsPerPage);
        Task<GetGenreDto> GetByIdAsync(Guid id);
        Task<GetGenreDto> AddAsync(CreateGenreDto createDto);
        Task<GetGenreDto> UpdateAsync(Guid id, UpdateGenreDto updateDto);
        Task DeleteAsync(Guid id);
    }
}
