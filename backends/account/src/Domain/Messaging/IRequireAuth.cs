namespace Domain.Messaging;

public interface IRequireAuth 
{
    public string? TokenId { get; set; } 
    public string? TokenEmail { get; set; }
}
