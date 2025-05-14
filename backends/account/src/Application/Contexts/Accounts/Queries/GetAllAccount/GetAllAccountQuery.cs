using Application.Contexts.Accounts.Dtos;
using Domain.Messaging;
using MediatR;

namespace Application.Contexts.Accounts.Queries.GetAllAccount;

public class GetAllAccountQuery : IRequest<IReadOnlyCollection<AccountDto>>, IRequireManager
{
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}