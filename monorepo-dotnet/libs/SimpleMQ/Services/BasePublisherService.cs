using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using SimpleMq.Interfaces;
using SimpleMq.Options;

namespace SimpleMq.Services;

public abstract class BasePublisherService(IConnectionService connectionService)
{
    protected async Task PublishMessageAsync<T>(
        string exchange,
        string routingKey,
        T message,
        PublishMessageOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        options ??= new PublishMessageOptions();

        var messageJson = JsonSerializer.Serialize(message, options.SerializerOptions);
        var body = Encoding.UTF8.GetBytes(messageJson);

        var proprieties = new BasicProperties();
        proprieties.Persistent = options.Persistent;
        proprieties.MessageId = string.IsNullOrWhiteSpace(options.MessageId)
            ? Guid.NewGuid().ToString()
            : options.MessageId;
        proprieties.CorrelationId = options.CorrelationId;
        proprieties.ContentType = options.ContentType;
        proprieties.ContentEncoding = options.ContentEncoding;
        proprieties.Headers = options.Headers;

        await using var channel = await connectionService.OpenChannel(cancellationToken);
        await channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            mandatory: options.Mandatory,
            basicProperties: proprieties,
            body: body,
            cancellationToken: cancellationToken
        );

    }
}