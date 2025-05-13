using Domain.Messaging;
using MediatR;

namespace Application.Contexts.Accounts.Queries.EnsureAccountOwner;

public class EnsureAccountOwnerQuery: IRequest, IRequireRegular
{
    public string? AccountId { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}