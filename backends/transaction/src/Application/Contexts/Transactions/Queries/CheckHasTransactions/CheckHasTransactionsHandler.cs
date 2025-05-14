using Application.Contexts.Transactions.Dtos;
using Application.Contexts.Transactions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;

namespace Application.Contexts.Transactions.Queries.CheckHasTransactionsManager;

public class CheckHasTransactionsHandler : IRequestHandler<CheckHasTransactionsQuery>
{
    private readonly ITransactionRepository  _transactionRepository;

    public CheckHasTransactionsHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task Handle(CheckHasTransactionsQuery request, CancellationToken cancellationToken)
    {
        var accountId = Transaction.ValidateAccountId(request.AccountId);
        var hasTransactions = await _transactionRepository.CheckHasTransactionsAsync(accountId, cancellationToken);
        if (!hasTransactions)
        {
            throw new NotFoundCustomException("Not Found Transactions for this account");
        }
    }
}