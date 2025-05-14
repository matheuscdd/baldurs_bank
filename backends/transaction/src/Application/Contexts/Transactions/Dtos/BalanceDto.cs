using Domain.Enums;

namespace Application.Contexts.Transactions.Dtos;

public class BalanceDto
{ 
        public Guid AccountId { get; set; }
        public decimal Balance { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}