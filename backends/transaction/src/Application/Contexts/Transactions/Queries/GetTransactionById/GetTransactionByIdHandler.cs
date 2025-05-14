using Application.Contexts.Transactions.Dtos;
using Application.Contexts.Transactions.Repositories;
using Domain.Entities;
using Domain.Exceptions;
using Mapster;
using MediatR;

namespace Application.Contexts.Transactions.Queries.GetTransactionById;

public class GetTransactionByIdHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
{
    private readonly ITransactionRepository  _transactionRepository;

    public GetTransactionByIdHandler(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public async Task<TransactionDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transactionId = Transaction.ValidateId(request.TransactionId);
        var entity = await _transactionRepository.GetByIdAsync(transactionId, cancellationToken);
        if (entity == null)
        {
            throw new NotFoundCustomException("Transaction Not Found");
        }
        var dto = entity.Adapt<TransactionDto>();
        return dto;
    }
}