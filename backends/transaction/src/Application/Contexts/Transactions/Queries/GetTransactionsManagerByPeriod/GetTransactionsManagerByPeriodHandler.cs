using Application.Contexts.Transactions.Dtos;
using Application.Contexts.Transactions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using MediatR;

namespace Application.Contexts.Transactions.Queries.GetTransactionsManagerByPeriod;

public class GetTransactionsManagerByPeriodHandler : IRequestHandler<GetTransactionsManagerByPeriodQuery, IReadOnlyCollection<TransactionDto>>
{
    private readonly ITransactionRepository  _transactionRepository;

    public GetTransactionsManagerByPeriodHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<IReadOnlyCollection<TransactionDto>> Handle(GetTransactionsManagerByPeriodQuery request, CancellationToken cancellationToken)
    {
        var accountId = Transaction.ValidateAccountId(request.AccountId);
        var startDate = DateTime.SpecifyKind(Transaction.ValidateDateTime(request.StartDate), DateTimeKind.Utc);
        var endDate = DateTime.SpecifyKind(Transaction.ValidateDateTime(request.EndDate), DateTimeKind.Utc);
        if (startDate >= endDate)
        {
            throw new ValidationCustomException("StartDate must be before EndDate");
        }
        var entities = await _transactionRepository.GetByPeriodAsync(accountId, startDate, endDate, cancellationToken);
        if (entities.Count == 0)
        {
            throw new NotFoundCustomException("No transactions found for this period");
        }
        var dtos = entities.Adapt<IReadOnlyCollection<TransactionDto>>();
        return dtos;
    }
}