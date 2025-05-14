using Domain.Entities;

namespace Application.Contexts.Accounts.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Account?> GetByIdByStatusAsync(Guid id, bool isActive, CancellationToken cancellationToken = default);
    Task<Account?> GetByNumberByStatusAsync(int number, bool isActive, CancellationToken cancellationToken = default);
    Task<bool> CheckUserIdByIdAsync(Guid accountId, string userId, CancellationToken cancellationToken = default);
    Task<Account?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<Account> ActiveAsync(Account entityStorage, CancellationToken cancellationToken = default);
    Task<Account> CreateAsync(Account entityRequest, CancellationToken cancellationToken = default);
    Task DisableAsync(Account entityRequest, CancellationToken cancellationToken = default);
    Task DeleteAsync(Account entityRequest, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Account>> GetAllAsync(CancellationToken cancellationToken = default);
}