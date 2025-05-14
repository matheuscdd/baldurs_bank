using Domain.Entities;

namespace Application.Contexts.Transactions.Repositories;

public interface ITransactionRepository
{
    Task<bool> CheckSufficientBalanceAsync(Guid accountId, decimal value, CancellationToken cancellationToken = default);
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<Transaction>> GetByPeriodAsync(Guid accountId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    Task<bool> CheckHasTransactionsAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<decimal> GetBalanceAsync(Guid accountId, CancellationToken cancellationToken = default);
    Task<Transaction> CreateAsync(Transaction entityRequest, CancellationToken cancellationToken = default);
}