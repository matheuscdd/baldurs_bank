using Application.Contexts.Accounts.Dtos;
using Domain.Messaging;
using MediatR;

namespace Application.Contexts.Accounts.Queries.GetAccountByNumber;

public class GetAccountByNumberQuery: IRequest<AccountDto>, IRequireAuth
{
    public int Number { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}