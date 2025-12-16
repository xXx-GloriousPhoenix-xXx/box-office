using AutoMapper;
using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.AuthorDtos;
using BoxOffice.BLL.Services.Interfaces;
using BoxOffice.BLL.Exceptions;
using BoxOffice.DAL.Models.Entities;
using BoxOffice.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoxOffice.BLL.Services.Implementations
{
    public partial class AuthorService(
        IUnitOfWork unitOfWork, 
        IMapper mapper,
        ILogger<AuthorService> logger) 
        : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        [LoggerMessage(LogLevel.Information, "Author created: {Name} (ID: {Id})")]
        partial void LogAuthorCreated(string name, Guid id);

        [LoggerMessage(LogLevel.Information, "Author updated: {Name} (ID: {Id})")]
        partial void LogAuthorUpdated(string name, Guid id);

        [LoggerMessage(LogLevel.Information, "Author deleted: {Name} (ID: {Id})")]
        partial void LogAuthorDeleted(string name, Guid id);

        public async Task<GetAuthorDto> AddAsync(CreateAuthorDto createDto, CancellationToken ct = default)
        {
            var existingAuthor = await _unitOfWork.Authors
                .ExistsAsync(a => a.Name.ToLower() == createDto.Name.ToLower(), ct);

            if (existingAuthor)
            {
                throw new ValidationException($"Author with name '{createDto.Name}' already exists");
            }

            var author = _mapper.Map<Author>(createDto);

            _unitOfWork.Authors.Add(author);
            await _unitOfWork.CompleteAsync(ct);

            LogAuthorCreated(author.Name, author.Id);

            return _mapper.Map<GetAuthorDto>(author);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var author = await _unitOfWork.Authors
                .GetByIdAsync(id, ct, includes: a => a.Posters);

            if (author is null)
            {
                throw new NotFoundException($"Author with id {id} not found");
            }

            if (author.Posters.Count != 0)
            {
                var postersCount = author.Posters.Count;
                throw new BusinessException(
                    $"Cannot delete author with {postersCount} posters. " +
                    "Delete or reassign posters first.");
            }

            _unitOfWork.Authors.Delete(author);
            await _unitOfWork.CompleteAsync(ct);

            LogAuthorDeleted(author.Name, author.Id);
        }

        public async Task<PagedResponse<GetAuthorDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default)
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

            var query = _unitOfWork.Authors.AsQueryable();
            var totalCount = await query.CountAsync(ct);

            var authors = await query
                .OrderBy(a => a.Name)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync(ct);

            var authorDtos = _mapper.Map<List<GetAuthorDto>>(authors);

            return new PagedResponse<GetAuthorDto>
            {
                Items = authorDtos,
                CurrentPage = page,
                PageSize = itemsPerPage,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)itemsPerPage)
            };
        }

        public async Task<GetAuthorWithPostersDto> GetAuthorPostersAsync(Guid id, CancellationToken ct = default)
        {
            var author = await _unitOfWork.Authors
                .GetByIdAsync(id, ct, includes: a => a.Posters);

            if (author == null)
                throw new NotFoundException($"Author with id {id} not found");

            var result = _mapper.Map<GetAuthorWithPostersDto>(author);

            foreach (var poster in author.Posters)
            {
                var posterWithGenres = await _unitOfWork.Posters
                    .GetByIdAsync(poster.Id, ct, includes: p => p.Genres);

                if (posterWithGenres != null)
                {
                    var posterDto = result.Posters.First(p => p.Id == poster.Id);
                    posterDto.Genres = [.. posterWithGenres.Genres.Select(g => g.Name)];
                }
            }

            return result;
        }

        public async Task<GetAuthorDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id, ct);
            if (author is null)
            {
                throw new NotFoundException($"Author with id {id} not found");
            }

            return _mapper.Map<GetAuthorDto>(author);
        }

        public async Task<GetAuthorDto> UpdateAsync(Guid id, UpdateAuthorDto updateDto, CancellationToken ct = default)
        {
            var author = await _unitOfWork.Authors.GetByIdAsync(id, ct);
            if (author is null)
            {
                throw new NotFoundException($"Author with id {id} not found");
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Name)
                && updateDto.Name != author.Name)
            {
                var nameExists = await _unitOfWork.Authors
                    .ExistsAsync(a => a.Name.ToLower() == updateDto.Name.ToLower().Trim() 
                           && a.Id != id, ct);

                if (nameExists)
                {
                    throw new ValidationException($"Author with name '{updateDto.Name}' already exists");
                }
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Name))
            {
                author.Name = updateDto.Name;
            }

            _unitOfWork.Authors.Update(author);
            await _unitOfWork.CompleteAsync(ct);

            LogAuthorUpdated(author.Name, author.Id);

            return _mapper.Map<GetAuthorDto>(author);
        }
    }
}
