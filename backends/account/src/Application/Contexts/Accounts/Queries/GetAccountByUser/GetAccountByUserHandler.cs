using Application.Contexts.Accounts.Dtos;
using Application.Contexts.Accounts.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using MediatR;

namespace Application.Contexts.Accounts.Queries.GetAccountByUser;

public class GetAccountByUserHandler: IRequestHandler<GetAccountByUserQuery, AccountDto>
{
    private readonly IAccountRepository  _accountRepository;

    public GetAccountByUserHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<AccountDto> Handle(
        GetAccountByUserQuery request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _accountRepository.GetByUserIdAsync(request.UserId, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundCustomException("Account not found");
        }
        
        var dto = entity.Adapt<AccountDto>();
        return dto;
    }
}