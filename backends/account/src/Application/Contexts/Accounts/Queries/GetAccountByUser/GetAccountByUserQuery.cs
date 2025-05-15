using Application.Contexts.Accounts.Dtos;
using Domain.Messaging;
using MediatR;

namespace Application.Contexts.Accounts.Queries.GetAccountByUser;

public class GetAccountByUserQuery: IRequest<AccountDto>, IRequireAuth
{
    public string UserId { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}