using Application.Contexts.Accounts.Dtos;
using Application.Contexts.Accounts.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using MediatR;

namespace Application.Contexts.Accounts.Commands.DeleteManager;

public class DeleteManagerAccountHandler : IRequestHandler<DeleteManagerAccountCommand>
{
    private readonly IAccountRepository  _accountRepository;

    public DeleteManagerAccountHandler(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    public async Task Handle(
        DeleteManagerAccountCommand request,
        CancellationToken cancellationToken
    )
    {
        var accountId = Account.ValidateId(request.AccountId);
        var entity = await _accountRepository.GetByIdAsync(accountId, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundCustomException("Account Not Found");
        }

        await _accountRepository.DeleteAsync(entity, cancellationToken);
    }
}