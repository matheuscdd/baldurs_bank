using Application.Contexts.Accounts.Dtos;
using MediatR;
using Domain.Messaging;

namespace Application.Contexts.Accounts.Commands.DisableManager;

public class DisableManagerAccountCommand : IRequest, IRequireManager
{
    public string? AccountId { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}