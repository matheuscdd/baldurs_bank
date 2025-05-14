using Application.Contexts.Transactions.Dtos;
using Domain.Messaging;
using MediatR;

namespace Application.Contexts.Transactions.Queries.GetTransactionsRegularByPeriod;

public class GetTransactionsRegularByPeriodQuery : IRequest<IReadOnlyCollection<TransactionDto>>, IRequireRegular
{
    public string AccountId { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}