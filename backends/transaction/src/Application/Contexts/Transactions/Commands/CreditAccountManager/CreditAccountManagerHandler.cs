using Application.Contexts.Transactions.Dtos;
using Application.Contexts.Transactions.Repositories;
using Domain.Entities;
using Domain.Enums;
using Mapster;
using MediatR;

namespace Application.Contexts.Transactions.Commands.CreditAccountManager;

public class CreditAccountManagerHandler : IRequestHandler<CreditAccountManagerCommand, TransactionDto>
{
    private readonly ITransactionRepository  _transactionRepository;

    public CreditAccountManagerHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<TransactionDto> Handle(CreditAccountManagerCommand request, CancellationToken cancellationToken)
    {
        var entity = await _transactionRepository.CreateAsync(
            new Transaction(request.AccountId, request.Value, Method.Credit),
            cancellationToken
        );
        var dto = entity.Adapt<TransactionDto>();
        return dto;
    }
}