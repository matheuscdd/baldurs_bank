using Application.Contexts.Accounts.Dtos;
using Application.Contexts.Accounts.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using MediatR;

namespace Application.Contexts.Accounts.Queries.GetAccountById;

public class GetAccountByIdHandler: IRequestHandler<GetAccountByIdQuery, AccountDto>
{
    private readonly IAccountRepository  _accountRepository;

    public GetAccountByIdHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<AccountDto> Handle(
        GetAccountByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var entity = new Account(request.AccountId);
        entity = await _accountRepository.GetByIdByStatusAsync(entity.Id, true, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundCustomException("No active accounts were found with these credentials");
        }
        
        var dto = entity.Adapt<AccountDto>();
        return dto;
    }
}