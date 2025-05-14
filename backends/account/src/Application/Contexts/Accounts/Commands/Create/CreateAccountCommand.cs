using Application.Contexts.Accounts.Dtos;
using MediatR;
using Domain.Messaging;

namespace Application.Contexts.Accounts.Commands.Create;

public class CreateAccountCommand : IRequest<AccountDto>, IRequireRegular
{
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}