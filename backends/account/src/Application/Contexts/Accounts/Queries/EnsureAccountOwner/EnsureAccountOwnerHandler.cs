using Application.Contexts.Accounts.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Contexts.Accounts.Queries.EnsureAccountOwner;

public class EnsureAccountOwnerHandler: IRequestHandler<EnsureAccountOwnerQuery>
{
    private readonly IAccountRepository  _accountRepository;

    public EnsureAccountOwnerHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task Handle(
        EnsureAccountOwnerQuery request,
        CancellationToken cancellationToken
    )
    {
        var entity = new Account(request.AccountId, request.TokenId);
        var accountExists = await _accountRepository.CheckUserIdByIdAsync(entity.Id, entity.UserId);
        if (!accountExists)
        {
            throw new NotFoundCustomException("No active accounts were found with these credentials");
        }
        
    }
}