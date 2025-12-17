using AutoMapper;
using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TicketInfoDtos;
using BoxOffice.BLL.Exceptions;
using BoxOffice.BLL.Services.Interfaces;
using BoxOffice.DAL.Models.Entities;
using BoxOffice.DAL.Models.Enums;
using BoxOffice.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoxOffice.BLL.Services.Implementations
{
    public partial class TicketInfoService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<TicketInfoService> logger)
        : ITicketInfoService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        [LoggerMessage(LogLevel.Information, "TicketInfo created for Poster: {PosterId} (ID: {Id})")]
        partial void LogTicketInfoCreated(Guid posterId, Guid id);

        [LoggerMessage(LogLevel.Information, "TicketInfo updated: ID: {Id}")]
        partial void LogTicketInfoUpdated(Guid id);

        [LoggerMessage(LogLevel.Information, "TicketInfo deleted: ID: {Id}")]
        partial void LogTicketInfoDeleted(Guid id);

        public async Task<GetTicketInfoDto> AddAsync(CreateTicketInfoDto createDto, CancellationToken ct = default)
        {
            var poster = await _unitOfWork.Posters.GetByIdAsync(createDto.PosterId, ct);
            if (poster == null)
            {
                throw new NotFoundException($"Poster with id {createDto.PosterId} not found");
            }

            var ticketInfoExists = await _unitOfWork.TicketInfos
                .ExistsAsync(ti => ti.PosterId == createDto.PosterId
                    && ti.TicketType == createDto.TicketType, ct);

            if (ticketInfoExists)
            {
                throw new ValidationException(
                    $"TicketInfo with type '{createDto.TicketType}' already exists for poster {createDto.PosterId}");
            }

            if (createDto.TotalCount <= 0)
            {
                throw new ValidationException("TotalCount must be greater than 0");
            }

            if (createDto.Price <= 0)
            {
                throw new ValidationException("Price must be greater than 0");
            }

            var ticketInfo = _mapper.Map<TicketInfo>(createDto);
            ticketInfo.AvailableCount = createDto.TotalCount;
            ticketInfo.SoldCount = 0;
            ticketInfo.BookedCount = 0;

            _unitOfWork.TicketInfos.Add(ticketInfo);
            await _unitOfWork.CompleteAsync(ct);

            LogTicketInfoCreated(ticketInfo.PosterId, ticketInfo.Id);

            return _mapper.Map<GetTicketInfoDto>(ticketInfo);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var ticketInfo = await _unitOfWork.TicketInfos
                .GetByIdAsync(id, ct,
                    includes: ti => ti.Tickets);

            if (ticketInfo is null)
            {
                throw new NotFoundException($"TicketInfo with id {id} not found");
            }

            if (ticketInfo.Tickets.Count > 0)
            {
                var ticketsCount = ticketInfo.Tickets.Count;
                var soldOrBookedTickets = ticketInfo.Tickets
                    .Count(t => t.TicketState == TicketState.Sold || t.TicketState == TicketState.Booked);

                if (soldOrBookedTickets > 0)
                {
                    throw new BusinessException(
                        $"Cannot delete TicketInfo with {soldOrBookedTickets} sold or booked tickets. " +
                        "Cancel or process those tickets first.");
                }

                foreach (var ticket in ticketInfo.Tickets)
                {
                    var ticketWithTransactions = await _unitOfWork.Tickets
                        .GetByIdAsync(ticket.Id, ct,
                            includes: t => t.Transactions!);

                    if (ticketWithTransactions?.Transactions?.Count > 0)
                    {
                        throw new BusinessException(
                            $"Cannot delete TicketInfo because ticket {ticket.Id} has transactions. " +
                            "Delete transactions first.");
                    }
                }

                await _unitOfWork.BeginTransactionAsync(ct);
                try
                {
                    foreach (var ticket in ticketInfo.Tickets)
                    {
                        _unitOfWork.Tickets.Delete(ticket);
                    }

                    _unitOfWork.TicketInfos.Delete(ticketInfo);

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
                _unitOfWork.TicketInfos.Delete(ticketInfo);
                await _unitOfWork.CompleteAsync(ct);
            }

            LogTicketInfoDeleted(ticketInfo.Id);
        }

        public async Task<PagedResponse<GetTicketInfoDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default)
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

            var query = _unitOfWork.TicketInfos.AsQueryable()
                .Include(ti => ti.Poster!);

            var totalCount = await query.CountAsync(ct);

            var ticketInfos = await query
                .OrderBy(ti => ti.Poster!.Date)
                .ThenBy(ti => ti.TicketType)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync(ct);

            var ticketInfoDtos = _mapper.Map<List<GetTicketInfoDto>>(ticketInfos);

            return new PagedResponse<GetTicketInfoDto>
            {
                Items = ticketInfoDtos,
                CurrentPage = page,
                PageSize = itemsPerPage,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)itemsPerPage)
            };
        }

        public async Task<GetTicketInfoDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var ticketInfo = await _unitOfWork.TicketInfos
                .GetByIdAsync(id, ct,
                    includes: ti => ti.Poster!);

            if (ticketInfo is null)
            {
                throw new NotFoundException($"TicketInfo with id {id} not found");
            }

            return _mapper.Map<GetTicketInfoDto>(ticketInfo);
        }

        public async Task<GetTicketInfoDto> UpdateAsync(Guid id, UpdateTicketInfoDto updateDto, CancellationToken ct = default)
        {
            var ticketInfo = await _unitOfWork.TicketInfos.GetByIdAsync(id, ct);
            if (ticketInfo is null)
            {
                throw new NotFoundException($"TicketInfo with id {id} not found");
            }

            if (updateDto.Price.HasValue)
            {
                if (updateDto.Price.Value <= 0)
                {
                    throw new ValidationException("Price must be greater than 0");
                }

                var hasSoldOrBookedTickets = await _unitOfWork.Tickets
                    .ExistsAsync(t => t.TicketInfoId == id
                        && (t.TicketState == TicketState.Sold || t.TicketState == TicketState.Booked), ct);

                if (hasSoldOrBookedTickets)
                {
                    throw new BusinessException(
                        "Cannot update price when there are sold or booked tickets. " +
                        "Consider creating a new TicketInfo type instead.");
                }

                ticketInfo.Price = updateDto.Price.Value;
            }


            _unitOfWork.TicketInfos.Update(ticketInfo);
            await _unitOfWork.CompleteAsync(ct);

            LogTicketInfoUpdated(ticketInfo.Id);

            return _mapper.Map<GetTicketInfoDto>(ticketInfo);
        }
    }
}
