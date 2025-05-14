using Application.Contexts.Transactions.Dtos;
using MediatR;
using Domain.Messaging;

namespace Application.Contexts.Transactions.Commands.DebitAccountManager;

public class DebitAccountManagerCommand : IRequest<TransactionDto>, IRequireManager
{
    public string AccountId { get; set; }
    public string Value { get; set; }
    public string? TokenId { get; set; }
    public string? TokenEmail { get; set; }
}