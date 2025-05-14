namespace Application.Contexts.Transactions.Queries;

public class TransactionQueryParams
{
    public string AccountId { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
}