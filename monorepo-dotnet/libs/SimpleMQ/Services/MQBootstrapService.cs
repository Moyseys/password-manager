using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleMq.Attributes;
using SimpleMq.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SimpleMq.Services;

public sealed class MQBootstrapService(
    IServiceProvider serviceProvider,
    ISetupMQService setupMQService,
    IConnectionService connectionService,
    ILogger<MQBootstrapService> logger) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ISetupMQService _setupMQService = setupMQService;
    private readonly IConnectionService _connectionService = connectionService;
    private readonly ILogger<MQBootstrapService> _logger = logger;
    private readonly List<IChannel> _consumerChannels = [];
    private Dictionary _cacheDelegate;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Setting up broker topology...");
        await _setupMQService.SetupBroker(_connectionService.Connection);
        _logger.LogInformation("Broker topology configured.");

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a =>
            {
                try { return a.GetTypes(); }
                catch (ReflectionTypeLoadException ex) { return ex.Types.OfType<Type>(); }
            })
            .Where(t => typeof(IMessageConsumer).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var type in types)
        {
            var methods = type.GetMethods()
                .Where(m => m.GetCustomAttribute<ConsumerAttribute>() is not null)
                .ToList();

            if (methods.Count == 0) continue;


            foreach (var method in methods)
            {
                var attribute = method.GetCustomAttribute<ConsumerAttribute>()!;
                await RegisterConsumer(await RegisterChannel(), attribute.QueueName, attribute.AutoAck, type, method);
            }
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Closing consumer channels...");
        foreach (var channel in _consumerChannels)
        {
            try { await channel.CloseAsync(); channel.Dispose(); }
            catch (Exception ex) { _logger.LogWarning(ex, "Error closing consumer channel."); }
        }
        await base.StopAsync(cancellationToken);
    }

    private async Task RegisterConsumer(
        IChannel channel, string queueName, bool autoAck, Type handlerType, MethodInfo method)
    {
        _logger.LogInformation(
            "Registering consumer for queue {Queue} -> {Handler}.{Method} (AutoAck: {AutoAck})",
            queueName,
            handlerType.Name,
            method.Name,
            autoAck);
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (_, ea) =>
        {
            var messageId = ea.BasicProperties.MessageId ?? ea.DeliveryTag.ToString();
            _logger.LogInformation("Received message {MessageId} from queue {Queue}.", messageId, queueName);
            try
            {
                var messagePayload = Encoding.UTF8.GetString(ea.Body.ToArray());

                using var scope = _serviceProvider.CreateScope();
                var instance = scope.ServiceProvider.GetRequiredService(handlerType);

                var methodParameters = method.GetParameters();
                var invokeArgs = Array.Empty<object?>();

                if (methodParameters.Length == 1)
                {
                    var parameterType = methodParameters[0].ParameterType;

                    object? parsedMessage;
                    if (parameterType == typeof(string))
                    {
                        parsedMessage = messagePayload;
                    }
                    else
                    {
                        parsedMessage = JsonSerializer.Deserialize(messagePayload, parameterType);
                        if (parsedMessage is null)
                        {
                            throw new InvalidOperationException(
                                $"Failed to deserialize payload for handler parameter type '{parameterType.FullName}'.");
                        }
                    }

                    invokeArgs = [parsedMessage];
                }

                if (instance is not null)
                {
                    var result = method.Invoke(instance, invokeArgs);
                    if (result is Task task) await task;
                }

                if (!autoAck)
                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false);

                _logger.LogInformation("Message {MessageId} processed successfully.", messageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message {MessageId} on queue {Queue}. Dead-lettering.", messageId, queueName);
                await channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: false);
            }
        };

        await channel.BasicConsumeAsync(queueName, autoAck: autoAck, consumer: consumer);
        _logger.LogInformation("Subscribed to queue {Queue} → {Method}.", queueName, method.Name);
    }

    private async Task<IChannel> RegisterChannel()
    {
        // Use one lightweight channel per consumer handler.
        var channel = await _connectionService.OpenChannel();
        await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
        _consumerChannels.Add(channel);
        return channel;
    }

}

internal class Dictionary
{
}