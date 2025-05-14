using Application.Contexts.Accounts.Dtos;
using Domain.Messaging;
using MediatR;

namespace Application.Contexts.Accounts.Queries.GetAccountById;

public class GetAccountByIdQuery: IRequest<AccountDto>, IRequireAuth
{
    public string? AccountId { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}