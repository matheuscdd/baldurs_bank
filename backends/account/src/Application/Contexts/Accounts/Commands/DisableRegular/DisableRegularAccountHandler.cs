using Application.Contexts.Accounts.Dtos;
using Application.Contexts.Accounts.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using MediatR;

namespace Application.Contexts.Accounts.Commands.DisableRegular;

public class DisableRegularAccountHandler : IRequestHandler<DisableRegularAccountCommand>
{
    private readonly IAccountRepository  _accountRepository;

    public DisableRegularAccountHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task Handle(
        DisableRegularAccountCommand request,
        CancellationToken cancellationToken
    )
    {
        var accountId = Account.ValidateId(request.AccountId);
        var entity = await _accountRepository.GetByIdAsync(accountId, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundCustomException("Account Not Found");
        }

        await _accountRepository.DisableAsync(entity, cancellationToken);
    }
}