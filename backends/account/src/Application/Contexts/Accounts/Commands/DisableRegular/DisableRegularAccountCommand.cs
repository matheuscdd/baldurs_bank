using Application.Contexts.Accounts.Dtos;
using MediatR;
using Domain.Messaging;

namespace Application.Contexts.Accounts.Commands.DisableRegular;

public class DisableRegularAccountCommand : IRequest, IRequireRegular
{
    public string? AccountId { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}