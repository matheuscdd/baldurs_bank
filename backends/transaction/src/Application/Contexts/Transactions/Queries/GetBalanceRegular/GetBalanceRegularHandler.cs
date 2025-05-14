using Application.Contexts.Transactions.Dtos;
using Application.Contexts.Transactions.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Contexts.Transactions.Queries.GetBalanceRegular;

public class GetBalanceRegularHandler : IRequestHandler<GetBalanceRegularQuery, BalanceDto>
{
    private readonly ITransactionRepository  _transactionRepository;

    public GetBalanceRegularHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<BalanceDto> Handle(GetBalanceRegularQuery request, CancellationToken cancellationToken)
    {
        var accountId = Transaction.ValidateAccountId(request.AccountId);
        var balance = await _transactionRepository.GetBalanceAsync(accountId, cancellationToken);
        var dto = new BalanceDto
        {
            AccountId = accountId,
            Balance = balance,
        };
        return dto;
    }
}