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
}