using AutoMapper;
using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.DtoEnums;
using BoxOffice.BLL.DTOs.PosterDtos;
using BoxOffice.BLL.Exceptions;
using BoxOffice.BLL.Services.Interfaces;
using BoxOffice.DAL.Models.Entities;
using BoxOffice.DAL.Models.Enums;
using BoxOffice.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoxOffice.BLL.Services.Implementations
{
    public partial class PosterService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<PosterService> logger)
        : IPosterService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        [LoggerMessage(LogLevel.Information, "Poster created: {Name} (ID: {Id})")]
        partial void LogPosterCreated(string name, Guid id);

        [LoggerMessage(LogLevel.Information, "Poster updated: {Name} (ID: {Id})")]
        partial void LogPosterUpdated(string name, Guid id);

        [LoggerMessage(LogLevel.Information, "Poster deleted: {Name} (ID: {Id})")]
        partial void LogPosterDeleted(string name, Guid id);

        public async Task<GetPosterWithTicketInfosDto> AddAsync(CreatePosterDto createDto, CancellationToken ct = default)
        {
            // Validate Author exists
            var author = await _unitOfWork.Authors.GetByIdAsync(createDto.AuthorId, ct);
            if (author == null)
            {
                throw new NotFoundException($"Author with id {createDto.AuthorId} not found");
            }

            // Validate all genres exist
            var genres = new List<Genre>();
            foreach (var genreName in createDto.Genres)
            {
                var genre = await _unitOfWork.Genres
                    .AsQueryable()
                    .FirstOrDefaultAsync(g => g.Name.ToLower() == genreName.ToLower(), ct);

                if (genre == null)
                {
                    throw new NotFoundException($"Genre '{genreName}' not found");
                }
                genres.Add(genre);
            }

            // Check if poster with same name already exists
            var posterExists = await _unitOfWork.Posters
                .ExistsAsync(p => p.Name.ToLower() == createDto.Name.ToLower(), ct);

            if (posterExists)
            {
                throw new ValidationException($"Poster with name '{createDto.Name}' already exists");
            }

            // Validate date is in the future
            if (createDto.Date < DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new ValidationException("Poster date must be in the future");
            }

            // Validate duration
            if (createDto.Duration <= 0)
            {
                throw new ValidationException("Duration must be greater than 0");
            }

            // Validate TicketInfos
            foreach (var ticketInfoDto in createDto.TicketInfos)
            {
                if (ticketInfoDto.TotalCount <= 0)
                {
                    throw new ValidationException($"TicketInfo {ticketInfoDto.TicketType}: TotalCount must be greater than 0");
                }
                if (ticketInfoDto.Price <= 0)
                {
                    throw new ValidationException($"TicketInfo {ticketInfoDto.TicketType}: Price must be greater than 0");
                }

                // Validate unique TicketType
                var typeCount = createDto.TicketInfos
                    .Count(t => t.TicketType == ticketInfoDto.TicketType);
                if (typeCount > 1)
                {
                    throw new ValidationException($"TicketType '{ticketInfoDto.TicketType}' appears multiple times");
                }
            }

            // Create poster
            var poster = _mapper.Map<Poster>(createDto);
            poster.Genres = genres;
            poster.Author = author;

            // Start transaction for creating poster with ticket infos and tickets
            await _unitOfWork.BeginTransactionAsync(ct);
            try
            {
                // Add poster first
                _unitOfWork.Posters.Add(poster);
                await _unitOfWork.CompleteAsync(ct); // Save to get poster Id

                // Create TicketInfos and individual Tickets
                foreach (var ticketInfoDto in createDto.TicketInfos)
                {
                    // Create TicketInfo
                    var ticketInfo = new TicketInfo
                    {
                        PosterId = poster.Id,
                        TicketType = ticketInfoDto.TicketType,
                        Price = ticketInfoDto.Price,
                        TotalCount = ticketInfoDto.TotalCount,
                        AvailableCount = ticketInfoDto.TotalCount, // Initially all are available
                        SoldCount = 0,
                        BookedCount = 0
                    };

                    _unitOfWork.TicketInfos.Add(ticketInfo);
                    await _unitOfWork.CompleteAsync(ct); // Save to get ticketInfo Id

                    // Create individual tickets
                    var tickets = new List<Ticket>();
                    for (int i = 1; i <= ticketInfoDto.TotalCount; i++)
                    {
                        var seatNumber = GenerateSeatNumber(ticketInfoDto.TicketType, i, ticketInfoDto.TotalCount);

                        var ticket = new Ticket
                        {
                            TicketInfoId = ticketInfo.Id,
                            SeatNumber = seatNumber,
                            TicketState = TicketState.Available,
                            SoldDate = null,
                            CustomerId = null,
                            BookingId = null
                        };

                        tickets.Add(ticket);
                    }

                    // Add all tickets for this ticketInfo
                    _unitOfWork.Tickets.AddRange(tickets);
                    await _unitOfWork.CompleteAsync(ct);
                }

                await _unitOfWork.CommitTransactionAsync(ct);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw;
            }

            LogPosterCreated(poster.Name, poster.Id);

            // Return created poster with ticket infos
            return await GetByIdAsync(poster.Id, ct);
        }
        private static string GenerateSeatNumber(TicketType ticketType, int seatNumber, int totalSeats)
        {
            string section = ticketType.ToString()[0].ToString();

            // Calculate row and seat
            int seatsPerRow = 20; // 20 seats per row
            int row = (seatNumber - 1) / seatsPerRow + 1;
            int seatInRow = (seatNumber - 1) % seatsPerRow + 1;

            return $"{section}{row:00}-{seatInRow:00}";
        }
        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var poster = await _unitOfWork.Posters
                .GetByIdAsync(id, ct,
                    includes: p => p.TicketInfos!);

            if (poster is null)
            {
                throw new NotFoundException($"Poster with id {id} not found");
            }

            // Check if poster has any TicketInfos
            if (poster.TicketInfos.Any())
            {
                // Check if any tickets exist for this poster
                var hasTickets = false;
                var ticketInfos = new List<TicketInfo>();

                foreach (var ticketInfo in poster.TicketInfos)
                {
                    var fullTicketInfo = await _unitOfWork.TicketInfos
                        .GetByIdAsync(ticketInfo.Id, ct,
                            includes: ti => ti.Tickets!);

                    if (fullTicketInfo != null && fullTicketInfo.Tickets.Any())
                    {
                        hasTickets = true;
                        ticketInfos.Add(fullTicketInfo);
                    }
                }

                if (hasTickets)
                {
                    // Check for sold or booked tickets
                    var soldOrBookedTickets = ticketInfos
                        .SelectMany(ti => ti.Tickets)
                        .Count(t => t.TicketState == TicketState.Sold || t.TicketState == TicketState.Booked);

                    if (soldOrBookedTickets > 0)
                    {
                        throw new BusinessException(
                            $"Cannot delete poster with {soldOrBookedTickets} sold or booked tickets. " +
                            "Cancel or process those tickets first.");
                    }

                    // Check for transactions
                    foreach (var ticketInfo in ticketInfos)
                    {
                        foreach (var ticket in ticketInfo.Tickets)
                        {
                            var ticketWithTransactions = await _unitOfWork.Tickets
                                .GetByIdAsync(ticket.Id, ct,
                                    includes: t => t.Transactions!);

                            if (ticketWithTransactions?.Transactions?.Any() == true)
                            {
                                throw new BusinessException(
                                    $"Cannot delete poster because ticket {ticket.Id} has transactions. " +
                                    "Delete transactions first.");
                            }
                        }
                    }

                    // Start transaction for deleting poster with all related data
                    await _unitOfWork.BeginTransactionAsync(ct);
                    try
                    {
                        // Delete all tickets
                        foreach (var ticketInfo in ticketInfos)
                        {
                            foreach (var ticket in ticketInfo.Tickets)
                            {
                                _unitOfWork.Tickets.Delete(ticket);
                            }
                            _unitOfWork.TicketInfos.Delete(ticketInfo);
                        }

                        _unitOfWork.Posters.Delete(poster);

                        await _unitOfWork.CompleteAsync(ct);
                        await _unitOfWork.CommitTransactionAsync(ct);
                    }
                    catch
                    {
                        await _unitOfWork.RollbackTransactionAsync(ct);
                        throw;
                    }
                }
                else
                {
                    // No tickets, just delete TicketInfos and poster
                    await _unitOfWork.BeginTransactionAsync(ct);
                    try
                    {
                        foreach (var ticketInfo in poster.TicketInfos)
                        {
                            _unitOfWork.TicketInfos.Delete(ticketInfo);
                        }

                        _unitOfWork.Posters.Delete(poster);

                        await _unitOfWork.CompleteAsync(ct);
                        await _unitOfWork.CommitTransactionAsync(ct);
                    }
                    catch
                    {
                        await _unitOfWork.RollbackTransactionAsync(ct);
                        throw;
                    }
                }
            }
            else
            {
                // No TicketInfos, just delete poster
                _unitOfWork.Posters.Delete(poster);
                await _unitOfWork.CompleteAsync(ct);
            }

            LogPosterDeleted(poster.Name, poster.Id);
        }

        public async Task<PagedResponse<GetPosterDto>> GetAllAsync(
    SearchPosterDto? dto, int page = 1, int itemsPerPage = 10, CancellationToken ct = default)
        {
            // Validate and adjust pagination parameters
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

            // Создаем базовый запрос с Includes
            IQueryable<Poster> query = _unitOfWork.Posters.AsQueryable()
                .Include(p => p.Author!)
                .Include(p => p.Genres);

            // Проверяем, нужен ли фильтр по цене
            bool needsPriceFilter = dto != null && (dto.PriceMin.HasValue || dto.PriceMax.HasValue);

            // Применяем фильтры, если они предоставлены
            if (dto != null)
            {
                // Применяем фильтры к базовому запросу
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    query = query.Where(p => p.Name.ToLower().Contains(dto.Name.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(dto.Venue))
                {
                    query = query.Where(p => p.Venue.ToLower().Contains(dto.Venue.ToLower()));
                }

                if (!string.IsNullOrWhiteSpace(dto.AuthorName))
                {
                    query = query.Where(p => p.Author!.Name.ToLower().Contains(dto.AuthorName.ToLower()));
                }

                if (dto.DateFrom.HasValue)
                {
                    query = query.Where(p => p.Date >= dto.DateFrom.Value);
                }

                if (dto.DateTo.HasValue)
                {
                    query = query.Where(p => p.Date <= dto.DateTo.Value);
                }

                if (dto.DurationMin.HasValue)
                {
                    query = query.Where(p => p.Duration >= dto.DurationMin.Value);
                }

                if (dto.DurationMax.HasValue)
                {
                    query = query.Where(p => p.Duration <= dto.DurationMax.Value);
                }

                if (dto.Genres != null && dto.Genres.Any())
                {
                    query = query.Where(p => p.Genres.Any(g => dto.Genres.Contains(g.Name)));
                }

                // Фильтр по цене - добавляем Include для TicketInfos если нужно
                if (needsPriceFilter)
                {
                    query = query.Include(p => p.TicketInfos!);
                    query = query.Where(p => p.TicketInfos!.Any(ti =>
                        (!dto.PriceMin.HasValue || ti.Price >= dto.PriceMin.Value) &&
                        (!dto.PriceMax.HasValue || ti.Price <= dto.PriceMax.Value)
                    ));
                }

                // Применяем сортировку
                query = ApplySorting(query, dto.SearchCriteria, dto.SortAscending);
            }
            else
            {
                // Сортировка по умолчанию по дате
                query = query.OrderByDescending(p => p.Date);
            }

            var totalCount = await query.CountAsync(ct);

            var posters = await query
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync(ct);

            // Маппим в базовые DTO
            var posterDtos = _mapper.Map<List<GetPosterDto>>(posters);

            return new PagedResponse<GetPosterDto>
            {
                Items = posterDtos,
                CurrentPage = page,
                PageSize = itemsPerPage,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)itemsPerPage)
            };
        }

        private static IQueryable<Poster> ApplySorting(
            IQueryable<Poster> query, PosterSortCriteria? criteria, bool ascending)
        {
            return criteria switch
            {
                PosterSortCriteria.Name => ascending
                    ? query.OrderBy(p => p.Name)
                    : query.OrderByDescending(p => p.Name),

                PosterSortCriteria.Date => ascending
                    ? query.OrderBy(p => p.Date)
                    : query.OrderByDescending(p => p.Date),

                PosterSortCriteria.Duration => ascending
                    ? query.OrderBy(p => p.Duration)
                    : query.OrderByDescending(p => p.Duration),

                PosterSortCriteria.Venue => ascending
                    ? query.OrderBy(p => p.Venue)
                    : query.OrderByDescending(p => p.Venue),

                PosterSortCriteria.Author => ascending
                    ? query.OrderBy(p => p.Author!.Name)
                    : query.OrderByDescending(p => p.Author!.Name),

                PosterSortCriteria.Price => ascending
                    ? query.OrderBy(p => p.TicketInfos!.Min(ti => ti.Price))
                    : query.OrderByDescending(p => p.TicketInfos!.Max(ti => ti.Price)),

                _ => query.OrderBy(p => p.Date) // Сортировка по умолчанию
            };
        }

        public async Task<GetPosterWithTicketInfosDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var poster = await _unitOfWork.Posters
                .AsQueryable()
                .Include(p => p.Author!)
                .Include(p => p.Genres)
                .Include(p => p.TicketInfos!)
                .FirstOrDefaultAsync(p => p.Id == id, ct);

            if (poster is null)
            {
                throw new NotFoundException($"Poster with id {id} not found");
            }

            var posterDto = _mapper.Map<GetPosterWithTicketInfosDto>(poster);
            posterDto.Author = poster.Author?.Name ?? string.Empty;
            posterDto.Genres = poster.Genres.Select(g => g.Name).ToList();
            posterDto.TicketInfos = _mapper.Map<List<GetTicketInfoAdditionDto>>(poster.TicketInfos);

            return posterDto;
        }

        public async Task<GetPosterWithTicketsDto> GetPosterTicketsAsync(Guid id, TicketState state, CancellationToken ct = default)
        {
            var poster = await _unitOfWork.Posters
                .AsQueryable()
                .Include(p => p.Author!)
                .Include(p => p.Genres)
                .Include(p => p.TicketInfos!)
                    .ThenInclude(ti => ti.Tickets!)
                .FirstOrDefaultAsync(p => p.Id == id, ct);

            if (poster is null)
            {
                throw new NotFoundException($"Poster with id {id} not found");
            }

            // Get all tickets for this poster with the specified state
            var allTickets = poster.TicketInfos
                .SelectMany(ti => ti.Tickets)
                .Where(t => t.TicketState == state)
                .ToList();

            // Map to DTO
            var posterDto = _mapper.Map<GetPosterWithTicketsDto>(poster);
            posterDto.Author = poster.Author?.Name ?? string.Empty;
            posterDto.Genres = poster.Genres.Select(g => g.Name).ToList();
            posterDto.Tickets = new PagedResponse<GetTicketAdditionDto>
            {
                Items = _mapper.Map<List<GetTicketAdditionDto>>(allTickets),
                CurrentPage = 1,
                PageSize = allTickets.Count,
                TotalCount = allTickets.Count,
                TotalPages = 1
            };

            // Add price and type to ticket DTOs
            foreach (var ticketDto in posterDto.Tickets.Items)
            {
                var ticket = allTickets.First(t => t.Id == ticketDto.Id);
                var ticketInfo = poster.TicketInfos.First(ti => ti.Id == ticket.TicketInfoId);
                ticketDto.Price = ticketInfo.Price;
                ticketDto.Type = ticketInfo.TicketType;
            }

            return posterDto;
        }

        public async Task<GetPosterStatsDto> GetPosterStatisticsAsync(Guid id, CancellationToken ct = default)
        {
            var poster = await _unitOfWork.Posters
                .AsQueryable()
                .Include(p => p.Author!)
                .Include(p => p.Genres)
                .Include(p => p.TicketInfos!)
                    .ThenInclude(ti => ti.Tickets!)
                        .ThenInclude(t => t.Transactions!)
                .FirstOrDefaultAsync(p => p.Id == id, ct);

            if (poster is null)
            {
                throw new NotFoundException($"Poster with id {id} not found");
            }

            // Calculate statistics
            var stats = new GetPosterStatsDto
            {
                Id = poster.Id,
                Name = poster.Name,
                Author = poster.Author?.Name ?? string.Empty,
                Description = poster.Description,
                Venue = poster.Venue,
                Date = poster.Date,
                Duration = poster.Duration,
                Genres = poster.Genres.Select(g => g.Name).ToList(),
                ReportTime = DateTime.UtcNow
            };

            // Aggregate ticket counts
            stats.CountTotal = poster.TicketInfos.Sum(ti => ti.TotalCount);
            stats.CountSold = poster.TicketInfos.Sum(ti => ti.SoldCount);
            stats.CountBooked = poster.TicketInfos.Sum(ti => ti.BookedCount);
            stats.CountAvailable = poster.TicketInfos.Sum(ti => ti.AvailableCount);

            // Calculate revenue (sum of purchase transactions)
            stats.Revenue = poster.TicketInfos
                .SelectMany(ti => ti.Tickets)
                .SelectMany(t => t.Transactions)
                .Where(tr => tr.TransactionType == TransactionType.Purchase)
                .Sum(tr => tr.Amount);

            return stats;
        }

        public async Task<GetPosterDto> UpdateAsync(Guid id, UpdatePosterDto updateDto, CancellationToken ct = default)
        {
            var poster = await _unitOfWork.Posters
                .GetByIdAsync(id, ct,
                    p => p.Author!,
                    p => p.Genres);

            if (poster is null)
            {
                throw new NotFoundException($"Poster with id {id} not found");
            }

            // Check if updating name and if new name is unique
            if (!string.IsNullOrWhiteSpace(updateDto.Name)
                && updateDto.Name != poster.Name)
            {
                var nameExists = await _unitOfWork.Posters
                    .ExistsAsync(p => p.Name.ToLower() == updateDto.Name.ToLower().Trim()
                           && p.Id != id, ct);

                if (nameExists)
                {
                    throw new ValidationException($"Poster with name '{updateDto.Name}' already exists");
                }
            }

            // Update Author if provided
            if (updateDto.AuthorId.HasValue)
            {
                var author = await _unitOfWork.Authors.GetByIdAsync(updateDto.AuthorId.Value, ct);
                if (author == null)
                {
                    throw new NotFoundException($"Author with id {updateDto.AuthorId} not found");
                }
                poster.Author = author;
            }

            // Update genres if provided
            if (updateDto.Genres != null)
            {
                var newGenres = new List<Genre>();
                foreach (var genreName in updateDto.Genres)
                {
                    var genre = await _unitOfWork.Genres
                        .AsQueryable()
                        .FirstOrDefaultAsync(g => g.Name.ToLower() == genreName.ToLower(), ct);

                    if (genre == null)
                    {
                        throw new NotFoundException($"Genre '{genreName}' not found");
                    }
                    newGenres.Add(genre);
                }
                poster.Genres = newGenres;
            }

            // Update other properties if provided
            if (!string.IsNullOrWhiteSpace(updateDto.Name))
            {
                poster.Name = updateDto.Name.Trim();
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Description))
            {
                poster.Description = updateDto.Description.Trim();
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Venue))
            {
                poster.Venue = updateDto.Venue.Trim();
            }

            if (updateDto.Date.HasValue)
            {
                if (updateDto.Date.Value < DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    throw new ValidationException("Poster date must be in the future");
                }
                poster.Date = updateDto.Date.Value;
            }

            if (updateDto.Duration.HasValue)
            {
                if (updateDto.Duration.Value <= 0)
                {
                    throw new ValidationException("Duration must be greater than 0");
                }
                poster.Duration = updateDto.Duration.Value;
            }

            _unitOfWork.Posters.Update(poster);
            await _unitOfWork.CompleteAsync(ct);

            LogPosterUpdated(poster.Name, poster.Id);

            // Return basic DTO
            var result = _mapper.Map<GetPosterDto>(poster);
            result.Author = poster.Author?.Name ?? string.Empty;
            result.Genres = poster.Genres.Select(g => g.Name).ToList();

            return result;
        }
    }
}