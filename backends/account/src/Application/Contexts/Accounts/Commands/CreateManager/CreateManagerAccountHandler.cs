using Application.Contexts.Accounts.Dtos;
using Application.Contexts.Accounts.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using MediatR;

namespace Application.Contexts.Accounts.Commands.CreateManager;

public class CreateManagerAccountHandler : IRequestHandler<CreateManagerAccountCommand, AccountDto>
{
    private readonly IAccountRepository  _accountRepository;

    public CreateManagerAccountHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task<AccountDto> Handle(
        CreateManagerAccountCommand request,
        CancellationToken cancellationToken
    )
    {
        var entity = await _accountRepository.GetByUserIdAsync(request.UserId, cancellationToken);
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
                new Account(request.UserId, true),
                cancellationToken
            );
        }

        var dto = entity.Adapt<AccountDto>();
        return dto;
    }
}