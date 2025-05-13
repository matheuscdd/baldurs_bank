using Application.Contexts.Accounts.Dtos;
using Application.Contexts.Accounts.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using MediatR;

namespace Application.Contexts.Accounts.Queries.GetAccountByNumber;

public class GetAccountByNumberHandler: IRequestHandler<GetAccountByNumberQuery, AccountDto>
{
    private readonly IAccountRepository  _accountRepository;

    public GetAccountByNumberHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<AccountDto> Handle(
        GetAccountByNumberQuery request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _accountRepository.GetByNumberByStatusAsync(request.Number, true, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundCustomException("No active accounts were found with these credentials");
        }
        
        var dto = entity.Adapt<AccountDto>();
        return dto;
    }
}