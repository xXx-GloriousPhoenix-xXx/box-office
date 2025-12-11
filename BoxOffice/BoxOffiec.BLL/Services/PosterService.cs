using AutoMapper;
using BoxOffice.BLL.DTO;
using BoxOffice.BLL.Interfaces;
using BoxOffice.DAL.Entities;
using BoxOffice.DAL.Enums;
using BoxOffice.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace BoxOffice.BLL.Services
{
    public class PosterService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PosterService> logger) : IPosterService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<PosterService> _logger = logger;

        public async Task<IEnumerable<PosterDto>> SearchPostersAsync(
            SearchPostersDto searchDto,
            CancellationToken ct = default)
        {
            var posters = await _unitOfWork.PosterRepository.SearchAsync(
                authorName: searchDto.AuthorName,
                title: searchDto.Title,
                genre: searchDto.Genre,
                dateFrom: searchDto.DateFrom,
                dateTo: searchDto.DateTo,
                ct: ct
            );

            return _mapper.Map<IEnumerable<PosterDto>>(posters);
        }

        public async Task<PosterDetailsDto?> GetPosterDetailsAsync(
            Guid posterId,
            CancellationToken ct = default)
        {
            var poster = await _unitOfWork.PosterRepository
                .GetPosterWithDetailsAsync(posterId, ct);

            if (poster == null)
                return null;

            var dto = _mapper.Map<PosterDetailsDto>(poster);

            dto.TotalTickets = poster.TicketInfos?.Sum(ti => ti.TotalTickets) ?? 0;
            dto.AvailableTickets = poster.TicketInfos?.Sum(ti => ti.AvailableTickets) ?? 0;
            dto.SoldTickets = poster.Tickets?.Count(t => t.State == TicketState.Sold) ?? 0;
            dto.BookedTickets = poster.Tickets?.Count(t => t.State == TicketState.Booked) ?? 0;

            return dto;
        }

        public async Task<PosterDto> CreatePosterAsync(
            CreatePosterDto createDto,
            CancellationToken ct = default)
        {
            await _unitOfWork.BeginTransactionAsync(ct);

            try
            {
                var author = await _unitOfWork.Authors.GetByIdAsync(createDto.AuthorId, ct)
                    ?? throw new ArgumentException($"Author with ID {createDto.AuthorId} not found");

                var poster = _mapper.Map<Poster>(createDto);
                poster.AuthorId = createDto.AuthorId;

                _unitOfWork.Posters.Add(poster);
                await _unitOfWork.CompleteAsync(ct);

                foreach (var ticketTypeDto in createDto.TicketTypes)
                {
                    var ticketInfo = new TicketInfo
                    {
                        PosterId = poster.Id,
                        TicketType = ticketTypeDto.Type,
                        Price = ticketTypeDto.Price,
                        TotalTickets = ticketTypeDto.TotalTickets,
                        AvailableTickets = ticketTypeDto.TotalTickets,
                        SoldTickets = 0,
                        BookedTickets = 0
                    };

                    _unitOfWork.TicketInfos.Add(ticketInfo);
                    await _unitOfWork.CompleteAsync(ct);

                    for (int i = 1; i <= ticketTypeDto.TotalTickets; i++)
                    {
                        var ticket = new Ticket
                        {
                            PosterId = poster.Id,
                            TicketInfoId = ticketInfo.Id,
                            SeatNumber = $"{GetSeatPrefix(ticketTypeDto.Type)}{i}",
                            State = TicketState.Available
                        };

                        _unitOfWork.Tickets.Add(ticket);
                    }
                }

                await _unitOfWork.CompleteAsync(ct);
                await _unitOfWork.CommitTransactionAsync(ct);

                return await GetPosterDetailsAsync(poster.Id, ct)
                    ?? throw new InvalidOperationException("Failed to create poster");
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                throw;
            }
        }

        private static string GetSeatPrefix(TicketType type) => type switch
        {
            TicketType.VIP => "V",
            TicketType.PremiumSeat => "P",
            TicketType.FrontRow => "F",
            TicketType.Balcony => "B",
            TicketType.Standard => "S",
            TicketType.Economy => "E",
            _ => "A"
        };

        public async Task<PosterDto> UpdatePosterAsync(
            Guid posterId,
            UpdatePosterDto updateDto,
            CancellationToken ct = default)
        {
            var poster = await _unitOfWork.Posters.GetByIdAsync(posterId, ct)
                ?? throw new ArgumentException($"Poster with ID {posterId} not found");

            if (!string.IsNullOrWhiteSpace(updateDto.Name))
                poster.Name = updateDto.Name;

            if (updateDto.Genres != null)
                poster.Genres = updateDto.Genres;

            if (updateDto.ReleaseDate.HasValue)
                poster.ReleaseDate = updateDto.ReleaseDate.Value;

            if (!string.IsNullOrWhiteSpace(updateDto.Venue))
                poster.Venue = updateDto.Venue;

            if (updateDto.DurationMinutes.HasValue)
                poster.DurationMinutes = updateDto.DurationMinutes.Value;

            if (!string.IsNullOrWhiteSpace(updateDto.Description))
                poster.Description = updateDto.Description;

            _unitOfWork.Posters.Update(poster);
            await _unitOfWork.CompleteAsync(ct);

            return _mapper.Map<PosterDto>(poster);
        }

        public async Task<bool> DeletePosterAsync(Guid posterId, CancellationToken ct = default)
        {
            var poster = await _unitOfWork.Posters.GetByIdAsync(posterId, ct);
            if (poster == null)
                return false;

            var hasActiveTickets = await _unitOfWork.Tickets.ExistsAsync(
                t => t.PosterId == posterId &&
                    (t.State == TicketState.Sold || t.State == TicketState.Booked),
                ct);

            if (hasActiveTickets)
                throw new InvalidOperationException("Cannot delete poster with active tickets");

            _unitOfWork.Posters.Delete(poster);
            await _unitOfWork.CompleteAsync(ct);

            return true;
        }

        public async Task<IEnumerable<PosterDto>> GetUpcomingPostersAsync(
            int daysAhead = 30,
            CancellationToken ct = default)
        {
            var posters = await _unitOfWork.PosterRepository
                .GetUpcomingPostersAsync(daysAhead, ct);

            return _mapper.Map<IEnumerable<PosterDto>>(posters);
        }
    }
}