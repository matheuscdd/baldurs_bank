using Application.Contexts.Transactions.Dtos;
using Application.Contexts.Transactions.Repositories;
using Domain.Entities;
using Domain.Enums;
using Mapster;
using MediatR;

namespace Application.Contexts.Transactions.Commands.CreditAccountRegular;

public class CreditAccountRegularHandler : IRequestHandler<CreditAccountRegularCommand, TransactionDto>
{
    private readonly ITransactionRepository  _transactionRepository;

    public CreditAccountRegularHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<TransactionDto> Handle(CreditAccountRegularCommand request, CancellationToken cancellationToken)
    {
        var entity = await _transactionRepository.CreateAsync(
            new Transaction(request.AccountId, request.Value, Method.Credit),
            cancellationToken
        );
        var dto = entity.Adapt<TransactionDto>();
        return dto;
    }
}