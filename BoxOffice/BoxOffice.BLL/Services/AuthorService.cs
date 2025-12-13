using AutoMapper;
using BoxOffice.BLL.DTO;
using BoxOffice.BLL.DTOs;
using BoxOffice.BLL.Interfaces;
using BoxOffice.DAL.Entities;
using BoxOffice.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace BoxOffice.BLL.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AuthorService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync(CancellationToken ct = default)
        {
            try
            {
                var authors = await _unitOfWork.Authors.GetAllAsync(ct);
                return _mapper.Map<IEnumerable<AuthorDto>>(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all authors");
                throw;
            }
        }

        public async Task<AuthorDto?> GetAuthorByIdAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                var author = await _unitOfWork.Authors.GetByIdAsync(id, ct);
                if (author == null)
                    return null;

                return _mapper.Map<AuthorDto>(author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting author with ID {AuthorId}", id);
                throw;
            }
        }

        public async Task<AuthorDto> CreateAuthorAsync(CreateAuthorDto createDto, CancellationToken ct = default)
        {
            try
            {
                var author = _mapper.Map<Author>(createDto);
                _unitOfWork.Authors.Add(author);
                await _unitOfWork.CompleteAsync(ct);

                return _mapper.Map<AuthorDto>(author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating author");
                throw;
            }
        }

        public async Task<AuthorDto> UpdateAuthorAsync(Guid id, UpdateAuthorDto updateDto, CancellationToken ct = default)
        {
            try
            {
                var author = await _unitOfWork.Authors.GetByIdAsync(id, ct);
                if (author == null)
                    throw new ArgumentException($"Author with ID {id} not found");

                _mapper.Map(updateDto, author);
                _unitOfWork.Authors.Update(author);
                await _unitOfWork.CompleteAsync(ct);

                return _mapper.Map<AuthorDto>(author);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating author with ID {AuthorId}", id);
                throw;
            }
        }

        public async Task<bool> DeleteAuthorAsync(Guid id, CancellationToken ct = default)
        {
            try
            {
                var author = await _unitOfWork.Authors.GetByIdAsync(id, ct);
                if (author == null)
                    return false;

                // Проверяем, есть ли у автора постеры
                var hasPosters = await _unitOfWork.Posters.ExistsAsync(p => p.AuthorId == id, ct);
                if (hasPosters)
                    throw new InvalidOperationException("Cannot delete author with existing posters");

                _unitOfWork.Authors.Delete(author);
                await _unitOfWork.CompleteAsync(ct);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting author with ID {AuthorId}", id);
                throw;
            }
        }
    }
}
