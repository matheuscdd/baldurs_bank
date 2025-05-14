using Domain.Enums;

namespace Application.Contexts.Transactions.Dtos;

public class TransactionDto
{ 
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public decimal Value { get; set; }
        public string Method { get; set; }
        public DateTime CreatedAt { get; set; }
}