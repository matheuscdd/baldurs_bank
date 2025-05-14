using Application.Contexts.Transactions.Dtos;
using MediatR;
using Domain.Messaging;

namespace Application.Contexts.Transactions.Queries.GetBalanceManager;

public class GetBalanceManagerQuery : IRequest<BalanceDto>, IRequireManager
{
    public string AccountId { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}