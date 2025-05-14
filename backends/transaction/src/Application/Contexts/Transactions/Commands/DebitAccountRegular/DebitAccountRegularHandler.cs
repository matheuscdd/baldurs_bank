using Application.Contexts.Transactions.Dtos;
using Application.Contexts.Transactions.Repositories;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Mapster;
using MediatR;

namespace Application.Contexts.Transactions.Commands.DebitAccountRegular;

public class DebitAccountRegularHandler : IRequestHandler<DebitAccountRegularCommand, TransactionDto>
{
    private readonly ITransactionRepository  _transactionRepository;

    public DebitAccountRegularHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<TransactionDto> Handle(DebitAccountRegularCommand request, CancellationToken cancellationToken)
    {
        var entity = new Transaction(request.AccountId, request.Value, Method.Debit);
        var hasEnoughBalance = await _transactionRepository.CheckSufficientBalanceAsync(entity.AccountId, entity.Value, cancellationToken);
        if (!hasEnoughBalance)
        {
            throw new ConflictCustomException("There is not enough balance");
        }
        entity = await _transactionRepository.CreateAsync(entity, cancellationToken);
        var dto = entity.Adapt<TransactionDto>();
        return dto;
    }
}