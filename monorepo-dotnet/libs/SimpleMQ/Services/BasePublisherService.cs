using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using SimpleMq.Interfaces;

namespace SimpleMq.Services;

public abstract class BasePublisherService(IConnectionService connectionService)
{
    protected async Task PublishMessageAsync<T>(
        string exchange,
        string routingKey,
        T message)
    {
        var messageJson = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageJson);

        var proprieties = new BasicProperties();
        proprieties.Persistent = true;
        proprieties.MessageId = Guid.NewGuid().ToString();
        proprieties.ContentType = "application/json";
        proprieties.ContentEncoding = "utf-8";

        await using var channel = await connectionService.OpenChannel();
        await channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: proprieties,
            body: body
        );

    }

    protected async Task PublishMessageAsync<T>(
        string exchange,
        string routingKey,
        bool mandatory,
        T message,
        Dictionary<string, object>? headers)
    {
        var messageJson = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(messageJson);

        var properties = new BasicProperties
        {
            Headers = headers,
            Persistent = true,
            MessageId = Guid.NewGuid().ToString(),
            ContentType = "application/json",
            ContentEncoding = "utf-8"
        };

        await using var channel = await connectionService.OpenChannel();
        await channel.BasicPublishAsync(
           exchange: exchange,
           routingKey: routingKey,
           mandatory: mandatory,
           basicProperties: properties,
           body: body
       );

    }
}