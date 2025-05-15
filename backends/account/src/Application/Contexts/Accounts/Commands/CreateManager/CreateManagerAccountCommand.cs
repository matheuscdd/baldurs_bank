using Application.Contexts.Accounts.Dtos;
using MediatR;
using Domain.Messaging;

namespace Application.Contexts.Accounts.Commands.CreateManager;

public class CreateManagerAccountCommand : IRequest<AccountDto>, IRequireManager
{
    public string? UserId { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}