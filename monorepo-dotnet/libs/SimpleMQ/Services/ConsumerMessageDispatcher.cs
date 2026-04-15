using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SimpleMq.Dtos;

namespace SimpleMq.Services;

public sealed class ConsumerMessageDispatcher(
    IServiceProvider serviceProvider,
    ILogger<ConsumerMessageDispatcher> logger)
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<ConsumerMessageDispatcher> _logger = logger;

    internal async Task HandleAsync(
        IChannel channel,
        ConsumerRegistration registration,
        BasicDeliverEventArgs ea,
        CancellationToken cancellationToken)
    {
        var messageId = ea.BasicProperties.MessageId ?? ea.DeliveryTag.ToString();
        _logger.LogInformation("Received message {MessageId} from queue {Queue}.", messageId, registration.QueueName);

        try
        {
            if (!string.IsNullOrWhiteSpace(registration.RoutingKey) &&
                !string.Equals(registration.RoutingKey, ea.RoutingKey, StringComparison.Ordinal))
            {
                _logger.LogWarning(
                    "Message {MessageId} arrived with unexpected routing key {ActualRoutingKey}. Expected {ExpectedRoutingKey}.",
                    messageId,
                    ea.RoutingKey,
                    registration.RoutingKey
                );

                if (!registration.AutoAck)
                {
                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken);
                }

                return;
            }

            var messagePayload = Encoding.UTF8.GetString(ea.Body.ToArray());

            await using var scope = _serviceProvider.CreateAsyncScope();
            var instance = scope.ServiceProvider.GetRequiredService(registration.HandlerType);

            object? parsedMessage = null;
            if (registration.PayloadType is not null)
            {
                if (registration.PayloadType == typeof(string))
                {
                    parsedMessage = messagePayload;
                }
                else
                {
                    parsedMessage = JsonSerializer.Deserialize(messagePayload, registration.PayloadType) ??
                        throw new InvalidOperationException(
                            $"Failed to deserialize payload for handler parameter type '{registration.PayloadType.FullName}'."
                        );
                }
            }

            await registration.InvokeAsync(instance, parsedMessage);

            if (!registration.AutoAck)
            {
                await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken);
            }

            _logger.LogInformation("Message {MessageId} processed successfully.", messageId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message {MessageId} on queue {Queue}.", messageId, registration.QueueName);
            if (!registration.AutoAck)
            {
                _logger.LogWarning("Message {MessageId} will be dead-lettered via NACK.", messageId);
                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false, cancellationToken);
            }
            else
            {
                _logger.LogWarning(
                    "Message {MessageId} failed with autoAck enabled, so it cannot be negatively acknowledged.",
                    messageId
                );
            }
        }
    }
}