using Application.Contexts.Transactions.Dtos;
using MediatR;
using Domain.Messaging;

namespace Application.Contexts.Transactions.Queries.GetTransactionsManagerByPeriod;

public class GetTransactionsManagerByPeriodQuery : IRequest<IReadOnlyCollection<TransactionDto>>, IRequireManager
{
    public string AccountId { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}