using Application.Contexts.Transactions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Context;

namespace Repository.Repositories.Accounts;

public class TransactionRepository: ITransactionRepository
{
    private readonly ApplicationDbContext _context;

    public TransactionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CheckSufficientBalanceAsync(Guid accountId, decimal value, CancellationToken cancellationToken = default)
    {
        var balance = await GetBalanceAsync(accountId, cancellationToken);
        return (balance - Math.Abs(value)) >= 0;
    }

    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Transactions.FindAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Transaction>> GetByPeriodAsync(Guid accountId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
    {
        return await _context.Transactions
            .Where(el => el.AccountId == accountId && el.CreatedAt >= startDate && el.CreatedAt <= endDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<decimal> GetBalanceAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions
            .Where(e => e.AccountId == accountId)
            .SumAsync(e => e.Value, cancellationToken);
    }

    public async Task<Transaction> CreateAsync(Transaction entityRequest, CancellationToken cancellationToken = default)
    {
        await _context.Transactions.AddAsync(entityRequest, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entityRequest;
    }

    public async Task<bool> CheckHasTransactionsAsync(Guid accountId, CancellationToken cancellationToken = default)
    {
        return await _context.Transactions.AnyAsync(el => el.AccountId == accountId, cancellationToken);
    }
}