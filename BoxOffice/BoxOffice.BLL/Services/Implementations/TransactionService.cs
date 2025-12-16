using AutoMapper;
using BoxOffice.BLL.DTOs.AdditionalDtos;
using BoxOffice.BLL.DTOs.TransactionDtos;
using BoxOffice.BLL.Exceptions;
using BoxOffice.BLL.Services.Interfaces;
using BoxOffice.DAL.Models.Entities;
using BoxOffice.DAL.Models.Enums;
using BoxOffice.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BoxOffice.BLL.Services.Implementations
{
    public partial class TransactionService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<TransactionService> logger)
        : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        [LoggerMessage(LogLevel.Information, "Transaction created: {Amount} {TransactionType} (ID: {Id})")]
        partial void LogTransactionCreated(decimal amount, TransactionType transactionType, Guid id);

        [LoggerMessage(LogLevel.Information, "Transaction deleted: ID: {Id}")]
        partial void LogTransactionDeleted(Guid id);

        public async Task<GetTransactionDto> AddAsync(CreateTransactionDto createDto, CancellationToken ct = default)
        {
            // Validate Customer exists
            var customer = await _unitOfWork.Customers.GetByIdAsync(createDto.CustomerId, ct);
            if (customer == null)
            {
                throw new NotFoundException($"Customer with id {createDto.CustomerId} not found");
            }

            // Validate Ticket exists
            var ticket = await _unitOfWork.Tickets
                .GetByIdAsync(createDto.TicketId, ct,
                    includes: t => t.TicketInfo!);

            if (ticket == null)
            {
                throw new NotFoundException($"Ticket with id {createDto.TicketId} not found");
            }

            // Validate amount
            if (createDto.Amount <= 0)
            {
                throw new ValidationException("Amount must be greater than 0");
            }

            // For purchase transactions, validate ticket state
            if (createDto.TransactionType == TransactionType.Purchase)
            {
                if (ticket.TicketState != TicketState.Sold)
                {
                    throw new BusinessException(
                        $"Cannot create purchase transaction for ticket in '{ticket.TicketState}' state. " +
                        "Ticket must be in Sold state.");
                }

                // Check if purchase transaction already exists for this ticket
                var existingPurchase = await _unitOfWork.Transactions
                    .ExistsAsync(t => t.TicketId == createDto.TicketId
                        && t.TransactionType == TransactionType.Purchase, ct);

                if (existingPurchase)
                {
                    throw new ValidationException("Purchase transaction already exists for this ticket");
                }

                // Validate amount matches ticket price
                if (ticket.TicketInfo != null && Math.Abs(createDto.Amount - ticket.TicketInfo.Price) > 0.01m)
                {
                    throw new ValidationException(
                        $"Transaction amount {createDto.Amount} doesn't match ticket price {ticket.TicketInfo.Price}");
                }
            }

            // For refund transactions, validate ticket was purchased
            if (createDto.TransactionType == TransactionType.Refund)
            {
                if (ticket.TicketState != TicketState.Sold)
                {
                    throw new BusinessException(
                        $"Cannot create refund transaction for ticket in '{ticket.TicketState}' state. " +
                        "Only sold tickets can be refunded.");
                }

                // Check if ticket has a purchase transaction
                var purchaseTransaction = await _unitOfWork.Transactions
                    .FindAsync(t => t.TicketId == createDto.TicketId
                        && t.TransactionType == TransactionType.Purchase, ct);

                if (!purchaseTransaction.Any())
                {
                    throw new BusinessException("Cannot refund ticket that doesn't have a purchase transaction");
                }

                // For refunds, amount should be negative or we can validate it matches purchase amount
                var purchaseAmount = purchaseTransaction.First().Amount;
                if (Math.Abs(createDto.Amount + purchaseAmount) > 0.01m)
                {
                    throw new ValidationException(
                        $"Refund amount {createDto.Amount} should be negative and match purchase amount {purchaseAmount}");
                }
            }

            // Create transaction
            var transaction = _mapper.Map<Transaction>(createDto);
            transaction.Date = DateOnly.FromDateTime(DateTime.UtcNow);

            // For refunds, update ticket state
            if (createDto.TransactionType == TransactionType.Refund)
            {
                await _unitOfWork.BeginTransactionAsync(ct);
                try
                {
                    // Update ticket state back to Available
                    ticket.TicketState = TicketState.Available;
                    ticket.SoldDate = null;
                    ticket.CustomerId = null;

                    // Update TicketInfo counts
                    if (ticket.TicketInfo != null)
                    {
                        ticket.TicketInfo.SoldCount--;
                        ticket.TicketInfo.AvailableCount++;
                        _unitOfWork.TicketInfos.Update(ticket.TicketInfo);
                    }

                    _unitOfWork.Tickets.Update(ticket);
                    _unitOfWork.Transactions.Add(transaction);

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
                // For purchase transactions, just add the transaction
                _unitOfWork.Transactions.Add(transaction);
                await _unitOfWork.CompleteAsync(ct);
            }

            LogTransactionCreated(transaction.Amount, transaction.TransactionType, transaction.Id);

            return _mapper.Map<GetTransactionDto>(transaction);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id, ct);

            if (transaction is null)
            {
                throw new NotFoundException($"Transaction with id {id} not found");
            }

            // Check transaction type - refunds might have side effects
            if (transaction.TransactionType == TransactionType.Refund)
            {
                // For refunds, we need to reverse the ticket state changes
                var ticket = await _unitOfWork.Tickets
                    .GetByIdAsync(transaction.TicketId, ct,
                        includes: t => t.TicketInfo!);

                if (ticket != null)
                {
                    await _unitOfWork.BeginTransactionAsync(ct);
                    try
                    {
                        // Reverse ticket state back to Sold
                        ticket.TicketState = TicketState.Sold;
                        ticket.SoldDate = DateOnly.FromDateTime(DateTime.UtcNow);
                        ticket.CustomerId = transaction.CustomerId;

                        // Update TicketInfo counts
                        if (ticket.TicketInfo != null)
                        {
                            ticket.TicketInfo.SoldCount++;
                            ticket.TicketInfo.AvailableCount--;
                            _unitOfWork.TicketInfos.Update(ticket.TicketInfo);
                        }

                        _unitOfWork.Tickets.Update(ticket);
                        _unitOfWork.Transactions.Delete(transaction);

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
                    _unitOfWork.Transactions.Delete(transaction);
                    await _unitOfWork.CompleteAsync(ct);
                }
            }
            else
            {
                // For purchase transactions, just delete
                _unitOfWork.Transactions.Delete(transaction);
                await _unitOfWork.CompleteAsync(ct);
            }

            LogTransactionDeleted(id);
        }

        public async Task<PagedResponse<GetTransactionDto>> GetAllAsync(int page = 1, int itemsPerPage = 10, CancellationToken ct = default)
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

            var query = _unitOfWork.Transactions.AsQueryable()
                .Include(t => t.Customer!)
                .Include(t => t.Ticket!);

            var totalCount = await query.CountAsync(ct);

            var transactions = await query
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.Id)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync(ct);

            var transactionDtos = _mapper.Map<List<GetTransactionDto>>(transactions);

            return new PagedResponse<GetTransactionDto>
            {
                Items = transactionDtos,
                CurrentPage = page,
                PageSize = itemsPerPage,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)itemsPerPage)
            };
        }

        public async Task<PagedResponse<GetTransactionDto>> GetCustomerTransactionsAsync(Guid customerId, int page = 1, int itemsPerPage = 10, CancellationToken ct = default)
        {
            // Validate customer exists
            var customerExists = await _unitOfWork.Customers
                .ExistsAsync(c => c.Id == customerId, ct);

            if (!customerExists)
            {
                throw new NotFoundException($"Customer with id {customerId} not found");
            }

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

            var query = _unitOfWork.Transactions.AsQueryable()
                .Where(t => t.CustomerId == customerId)
                .Include(t => t.Ticket!);

            var totalCount = await query.CountAsync(ct);

            var transactions = await query
                .OrderByDescending(t => t.Date)
                .ThenByDescending(t => t.Id)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .ToListAsync(ct);

            var transactionDtos = _mapper.Map<List<GetTransactionDto>>(transactions);

            return new PagedResponse<GetTransactionDto>
            {
                Items = transactionDtos,
                CurrentPage = page,
                PageSize = itemsPerPage,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)itemsPerPage)
            };
        }

        public async Task<ICollection<GetTransactionDto>> GetTicketTransactionsAsync(Guid ticketId, CancellationToken ct = default)
        {
            // Validate ticket exists
            var ticketExists = await _unitOfWork.Tickets
                .ExistsAsync(t => t.Id == ticketId, ct);

            if (!ticketExists)
            {
                throw new NotFoundException($"Ticket with id {ticketId} not found");
            }

            var transactions = await _unitOfWork.Transactions
                .FindAsync(t => t.TicketId == ticketId, ct);

            return _mapper.Map<List<GetTransactionDto>>(transactions);
        }

        public async Task<GetTransactionDto> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var transaction = await _unitOfWork.Transactions
                .GetByIdAsync(id, ct,
                    t => t.Customer!,
                    t => t.Ticket!);

            if (transaction is null)
            {
                throw new NotFoundException($"Transaction with id {id} not found");
            }

            return _mapper.Map<GetTransactionDto>(transaction);
        }
    }
}