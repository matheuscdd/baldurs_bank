namespace IoC.Messaging;

public class Envelope
{
    public string MessageType { get; set; }
    public string? Token { get; set; }
    public string? Payload { get; set; }
}
