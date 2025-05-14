using Application.Contexts.Accounts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Context;

namespace Repository.Repositories.Accounts;

public class AccountRepository: IAccountRepository
{
    private readonly ApplicationDbContext _context;

    public AccountRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Accounts.FindAsync(id, cancellationToken);
    }

    public async Task<Account?> GetByIdByStatusAsync(Guid id, bool isActive, CancellationToken cancellationToken = default)
    {
        return await _context.Accounts.FirstOrDefaultAsync(el => el.Id == id && el.IsActive == isActive, cancellationToken);
    }

    public async Task<Account?> GetByNumberByStatusAsync(int number, bool isActive, CancellationToken cancellationToken = default)
    {
        return await _context.Accounts.FirstOrDefaultAsync(el => el.Number == number && el.IsActive == isActive, cancellationToken);
    }

    public async Task<bool> CheckUserIdByIdAsync(Guid accountId, string userId, CancellationToken cancellationToken = default)
    {
        return await _context.Accounts.AnyAsync(
            el => el.UserId == userId && el.Id == accountId && el.IsActive == true, cancellationToken);
    }

    public async Task<bool> CheckUserIdExistsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _context.Accounts.AnyAsync(el => el.UserId == userId, cancellationToken);
    }

    public async Task<Account> CreateAsync(Account entityRequest, CancellationToken cancellationToken = default)
    {
        await _context.Accounts.AddAsync(entityRequest, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entityRequest;
    }

    public async Task<IReadOnlyCollection<Account>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Accounts.ToListAsync(cancellationToken);
    }

    public async Task DisableAsync(Account entityRequest, CancellationToken cancellationToken = default)
    {
        entityRequest.SetStatus(false);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Account entityRequest, CancellationToken cancellationToken = default)
    {
        _context.Accounts.Remove(entityRequest);
        await _context.SaveChangesAsync(cancellationToken);
    }
}