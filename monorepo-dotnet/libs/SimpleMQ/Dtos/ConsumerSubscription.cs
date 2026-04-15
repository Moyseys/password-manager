using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace SimpleMq.Dtos;

internal sealed record ConsumerSubscription(
    string QueueName,
    string ConsumerTag,
    IChannel Channel,
    AsyncEventingBasicConsumer Consumer,
    AsyncEventHandler<BasicDeliverEventArgs> Handler
);