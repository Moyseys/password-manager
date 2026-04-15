using System.Text.Json;

namespace SimpleMq.Options;

public sealed class PublishMessageOptions
{
    public bool Persistent { get; set; } = true;
    public bool Mandatory { get; set; }
    public string? MessageId { get; set; }
    public string? CorrelationId { get; set; }
    public string ContentType { get; set; } = "application/json";
    public string ContentEncoding { get; set; } = "utf-8";
    public IDictionary<string, object?>? Headers { get; set; }
    public JsonSerializerOptions? SerializerOptions { get; set; }
}