using Application.Contexts.Transactions.Dtos;
using Domain.Messaging;
using MediatR;

namespace Application.Contexts.Transactions.Commands.TransferAccountManager;

public class TransferAccountManagerCommand : IRequest<IReadOnlyCollection<TransactionDto>>, IRequireManager
{
    public string DestinationAccountId { get; set; }
    public string OriginAccountId { get; set; }
    public string Value { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}