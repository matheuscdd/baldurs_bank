namespace Domain.Messaging;

public class Envelope
{
    public string MessageType { get; set; }
    public string? Token { get; set; }
    public object? Payload { get; set; }
}
