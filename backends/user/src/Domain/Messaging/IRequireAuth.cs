namespace Domain.Messaging;

public interface IRequireAuth 
{
    public bool? TokenIsManager { get; set; }
    public string? TokenId { get; set; } 
    public string? TokenEmail { get; set; }
}
