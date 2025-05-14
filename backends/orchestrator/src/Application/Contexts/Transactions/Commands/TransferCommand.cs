namespace Application.Contexts.Transactions.Commands;

public class TransferCommand
{
    public string DestinationAccountId { get; set; }
    public string OriginAccountId { get; set; }
    public string Value { get; set; }
}