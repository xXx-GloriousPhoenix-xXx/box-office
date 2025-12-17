using AutoMapper;
using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.GenreDtos;
using BoxOffice.BLL.Exceptions;
using BoxOffice.BLL.Services.Interfaces;
using BoxOffice.DAL.Models.Entities;
using BoxOffice.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoxOffice.BLL.Services.Implementations
{
    public partial class GenreService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<GenreService> logger)
        : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        [LoggerMessage(LogLevel.Information, "Genre created: {Name} (ID: {Id})")]
        partial void LogGenreCreated(string name, Guid id);

        [LoggerMessage(LogLevel.Information, "Genre updated: {Name} (ID: {Id})")]
        partial void LogGenreUpdated(string name, Guid id);

        [LoggerMessage(LogLevel.Information, "Genre deleted: {Name} (ID: {Id})")]
        partial void LogGenreDeleted(string name, Guid id);

        public async Task<GetGenreDto> AddAsync(CreateGenreDto createDto, CancellationToken ct = default)
        {
            var existingGenre = await _unitOfWork.Genres
                .ExistsAsync(g => g.Name.ToLower() == createDto.Name.ToLower(), ct);

            if (existingGenre)
            {
                throw new ValidationException($"Genre with name '{createDto.Name}' already exists");
            }

            var genre = _mapper.Map<Genre>(createDto);

            _unitOfWork.Genres.Add(genre);
            await _unitOfWork.CompleteAsync(ct);

            LogGenreCreated(genre.Name, genre.Id);

            return _mapper.Map<GetGenreDto>(genre);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var genre = await _unitOfWork.Genres
                .GetByIdAsync(id, ct,
                    includes: g => g.Posters!);

            if (genre is null)
            {
                throw new NotFoundException($"Genre with id {id} not found");
            }

            if (genre.Posters.Count != 0)
            {
                var postersCount = genre.Posters.Count;
                throw new BusinessException(
                    $"Cannot delete genre with {postersCount} posters. " +
                    "Remove genre from posters first.");
            }

            _unitOfWork.Genres.Delete(genre);
            await _unitOfWork.CompleteAsync(ct);

            LogGenreDeleted(genre.Name, genre.Id);
        }

        public async Task<PagedResponse<GetGenreDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default)
        {
            if (page < 1)
            {
                page = 1;
            }
            if (itemsPerPage < 1)
            {
                itemsPerPage = 10;
            }
            if (itemsPerPage > 100)
            {
                itemsPerPage = 100;
            }

            var query = _unitOfWork.Genres.AsQueryable();
            var totalCount = await query.CountAsync(ct);

            var genres = await query
                .OrderBy(g => g.Name)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync(ct);

            var genreDtos = _mapper.Map<List<GetGenreDto>>(genres);

            return new PagedResponse<GetGenreDto>
            {
                Items = genreDtos,
                CurrentPage = page,
                PageSize = itemsPerPage,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)itemsPerPage)
            };
        }

        public async Task<GetGenreDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(id, ct);
            if (genre is null)
            {
                throw new NotFoundException($"Genre with id {id} not found");
            }

            return _mapper.Map<GetGenreDto>(genre);
        }

        public async Task<GetGenreDto> UpdateAsync(Guid id, UpdateGenreDto updateDto, CancellationToken ct = default)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(id, ct);
            if (genre is null)
            {
                throw new NotFoundException($"Genre with id {id} not found");
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Name)
                && updateDto.Name != genre.Name)
            {
                var nameExists = await _unitOfWork.Genres
                    .ExistsAsync(g => g.Name.ToLower() == updateDto.Name.ToLower().Trim()
                           && g.Id != id, ct);

                if (nameExists)
                {
                    throw new ValidationException($"Genre with name '{updateDto.Name}' already exists");
                }
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Name))
            {
                genre.Name = updateDto.Name.Trim();
            }

            _unitOfWork.Genres.Update(genre);
            await _unitOfWork.CompleteAsync(ct);

            LogGenreUpdated(genre.Name, genre.Id);

            return _mapper.Map<GetGenreDto>(genre);
        }
    }
}
