using System.Globalization;
using Application.Contexts.Transactions.Dtos;
using Application.Contexts.Transactions.Repositories;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Mapster;
using MediatR;

namespace Application.Contexts.Transactions.Commands.TransferAccountManager;

public class CreditAccountManagerHandler : IRequestHandler<TransferAccountManagerCommand, IReadOnlyCollection<TransactionDto>>
{
    private readonly ITransactionRepository  _transactionRepository;

    public CreditAccountManagerHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<IReadOnlyCollection<TransactionDto>> Handle(TransferAccountManagerCommand request, CancellationToken cancellationToken)
    {
        var value = Math.Abs(Transaction.ValidateFormatDecimal(request.Value, nameof(Transaction.Value)));
        var entityOrigin = new Transaction(request.OriginAccountId, (value * -1).ToString(CultureInfo.InvariantCulture), Method.Debit);
        var entityDestination = new Transaction(request.DestinationAccountId, value.ToString(CultureInfo.InvariantCulture), Method.Credit);
        if (entityOrigin.AccountId == entityDestination.AccountId)
        {
            throw new ValidationCustomException("OriginAccountId and DestinationAccountId need be different");
        }
        var hasEnoughBalance = await _transactionRepository.CheckSufficientBalanceAsync(entityOrigin.AccountId, entityOrigin.Value, cancellationToken);
        if (!hasEnoughBalance)
        {
            throw new ConflictCustomException("There is not enough balance");
        }
        entityOrigin = await _transactionRepository.CreateAsync(entityOrigin, cancellationToken);
        entityDestination = await _transactionRepository.CreateAsync(entityDestination, cancellationToken);
        var entities = new List<Transaction> { entityOrigin, entityDestination };
        var dtos = entities.Adapt<IReadOnlyCollection<TransactionDto>>();
        return dtos;
    }
}