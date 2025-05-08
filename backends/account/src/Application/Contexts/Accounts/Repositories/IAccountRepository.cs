using Domain.Entities;

namespace Application.Contexts.Accounts.Repositories;

public interface IAccountRepository
{
    // Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> CheckUserIdExistsAsync(string userId, CancellationToken cancellationToken = default);
    Task<Account> CreateAsync(Account entityRequest, CancellationToken cancellationToken = default);
    // Task<Account> DeleteAsync(Account entity, CancellationToken cancellationToken = default);
    // Task<Account> InactivateAsync(Account entity, CancellationToken cancellationToken = default);
}