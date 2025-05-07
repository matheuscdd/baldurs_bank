using Newtonsoft.Json.Linq;

namespace IoC.Messaging;

public class Envelope
{
    public string MessageType { get; set; }
    public string IdToken { get; set; }
    public JObject Payload { get; set; }
}
