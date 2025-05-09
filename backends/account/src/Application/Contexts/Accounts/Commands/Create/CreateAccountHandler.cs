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
        var accountAlreadyExists = await _accountRepository.CheckUserIdExistsAsync(request.TokenId, cancellationToken);
        if (accountAlreadyExists)
        {
            throw new ConflictCustomException("Account already exists");
        }

        var entity = await _accountRepository.CreateAsync(
            new Account(request.TokenId, true),
            cancellationToken
        );

        var dto = entity.Adapt<AccountDto>();
        return dto;
    }
}