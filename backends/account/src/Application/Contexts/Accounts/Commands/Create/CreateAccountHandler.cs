using Application.Contexts.Accounts.Dtos;
using Application.Contexts.Accounts.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using MediatR;

namespace Application.Contexts.Accounts.Commands.Create;

public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, AccountDto>
{
    private readonly IAccountRepository  _accountRepository;

    public CreateAccountHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountDto> Handle(
        CreateAccountCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _accountRepository.GetByUserIdAsync(request.TokenId, cancellationToken);
        if (entity != null && entity.IsActive)
        {
            throw new ConflictCustomException("Account already active");
        }

        if (entity != null)
        {
            entity = await _accountRepository.ActiveAsync(entity, cancellationToken);
        }
        else
        {
            entity = await _accountRepository.CreateAsync(
                new Account(request.TokenId, true),
                cancellationToken
            );
        }

        var dto = entity.Adapt<AccountDto>();
        return dto;
    }
}