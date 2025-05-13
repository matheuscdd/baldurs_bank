using Application.Contexts.Transactions.Dtos;
using MediatR;
using Domain.Messaging;

namespace Application.Contexts.Transactions.Queries.GetTransactionById;

public class GetTransactionByIdQuery : IRequest<TransactionDto>, IRequireManager
{
    public string TransactionId { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}