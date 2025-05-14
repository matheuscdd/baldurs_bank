using Application.Contexts.Transactions.Dtos;
using MediatR;
using Domain.Messaging;

namespace Application.Contexts.Transactions.Queries.GetBalanceRegular;

public class GetBalanceRegularQuery : IRequest<BalanceDto>, IRequireRegular
{
    public string AccountId { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}