using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleMq.Dtos;
using SimpleMq.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SimpleMq.Services;

public sealed class MQBootstrapService(
    ISetupMQService setupMQService,
    IConnectionService connectionService,
    IReadOnlyCollection<ConsumerRegistration> consumerRegistrations,
    ConsumerMessageDispatcher consumerMessageDispatcher,
    ILogger<MQBootstrapService> logger) : BackgroundService
{
    private readonly ISetupMQService _setupMQService = setupMQService;
    private readonly IConnectionService _connectionService = connectionService;
    private readonly IReadOnlyCollection<ConsumerRegistration> _consumerRegistrations = consumerRegistrations;
    private readonly ConsumerMessageDispatcher _consumerMessageDispatcher = consumerMessageDispatcher;
    private readonly ILogger<MQBootstrapService> _logger = logger;
    private readonly Dictionary<string, IChannel> _queueChannels = new(StringComparer.Ordinal);
    private readonly List<ConsumerSubscription> _consumerSubscriptions = [];

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Setting up broker topology...");
        var connection = await _connectionService.OpenConnection(stoppingToken);
        await _setupMQService.SetupBroker(connection);
        _logger.LogInformation("Broker topology configured.");

        if (_consumerRegistrations.Count == 0)
        {
            _logger.LogInformation("No consumers discovered. MQ bootstrap will stay idle.");
            return;
        }

        foreach (var registration in _consumerRegistrations)
        {
            stoppingToken.ThrowIfCancellationRequested();

            await RegisterConsumer(
                channel: await GetOrCreateQueueChannel(registration.QueueName, stoppingToken),
                registration: registration,
                cancellationToken: stoppingToken
            );
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cancelling consumers and closing queue channels...");

        foreach (var subscription in _consumerSubscriptions)
        {
            try
            {
                await subscription.Channel.BasicCancelAsync(subscription.ConsumerTag);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(
                    ex,
                    "Error cancelling consumer {ConsumerTag} for queue {Queue}.",
                    subscription.ConsumerTag,
                    subscription.QueueName
                );
            }

            subscription.Consumer.ReceivedAsync -= subscription.Handler;
        }

        foreach (var channel in _queueChannels.Values)
        {
            try
            {
                await channel.CloseAsync(cancellationToken);
                channel.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error closing consumer channel.");
            }
        }

        await base.StopAsync(cancellationToken);
    }

    private async Task RegisterConsumer(
        IChannel channel,
        ConsumerRegistration registration,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Registering consumer for queue {Queue} -> {Handler}.{Method} (AutoAck: {AutoAck}, RoutingKey: {RoutingKey})",
            registration.QueueName,
            registration.HandlerType.Name,
            registration.MethodName,
            registration.AutoAck,
            registration.RoutingKey ?? "<any>");

        var consumer = new AsyncEventingBasicConsumer(channel);
        AsyncEventHandler<BasicDeliverEventArgs> onMessage = async (_, ea) =>
            await _consumerMessageDispatcher.HandleAsync(channel, registration, ea, cancellationToken);
        consumer.ReceivedAsync += onMessage;

        var consumerTag = await channel.BasicConsumeAsync(
            registration.QueueName,
            autoAck: registration.AutoAck,
            consumer: consumer,
            cancellationToken: cancellationToken
        );

        _consumerSubscriptions.Add(new ConsumerSubscription(
            QueueName: registration.QueueName,
            ConsumerTag: consumerTag,
            Channel: channel,
            Consumer: consumer,
            Handler: onMessage
        ));

        _logger.LogInformation(
            "Subscribed to queue {Queue} -> {Method} with consumer tag {ConsumerTag}.",
            registration.QueueName,
            registration.MethodName,
            consumerTag
        );
    }

    private async Task<IChannel> GetOrCreateQueueChannel(string queueName, CancellationToken cancellationToken)
    {
        if (_queueChannels.TryGetValue(queueName, out var existingChannel))
        {
            return existingChannel;
        }

        var channel = await _connectionService.OpenChannel(cancellationToken);
        await channel.BasicQosAsync(
            prefetchSize: 0,
            prefetchCount: 1,
            global: false,
            cancellationToken: cancellationToken
        );

        _queueChannels[queueName] = channel;
        return channel;
    }

}