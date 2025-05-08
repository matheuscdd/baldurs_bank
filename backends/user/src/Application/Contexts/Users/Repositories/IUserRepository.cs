using Domain.Entities;

namespace Application.Contexts.Users.Repositories;

public interface IUserRepository
{
    Task<IReadOnlyCollection<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    Task<User> CreateAsync(User entityRequest, CancellationToken cancellationToken = default);
    // Task<User> UpdateAsync(User entityRequest, CancellationToken cancellationToken = default);
    // Task<User> DeleteAsync(User entity, CancellationToken cancellationToken = default);
    // Task<User?> GetByUserNameAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> CheckIdExistsAsync(string id, CancellationToken cancellationToken = default);
    Task<bool> CheckEmailExistsAsync(string email, CancellationToken cancellationToken = default);
}