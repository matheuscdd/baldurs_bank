using Application.Contexts.Accounts.Dtos;
using Application.Contexts.Accounts.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using MediatR;

namespace Application.Contexts.Accounts.Queries.GetAllAccount;

public class GetAllAccountHandler: IRequestHandler<GetAllAccountQuery, IReadOnlyCollection<AccountDto>>
{
    private readonly IAccountRepository  _accountRepository;

    public GetAllAccountHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }
    
    public async Task<IReadOnlyCollection<AccountDto>> Handle(
        GetAllAccountQuery request,
        CancellationToken cancellationToken
    )
    {
        var entities = await _accountRepository.GetAllAsync(cancellationToken);
        var dtos =  entities.Adapt<IReadOnlyCollection<AccountDto>>();
        return dtos;
    }
}