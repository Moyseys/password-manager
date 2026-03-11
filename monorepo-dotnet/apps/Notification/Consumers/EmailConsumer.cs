using RabbitMQ.Client;
using SimpleMq.Attributes;
using SimpleMq.Enums;
using SimpleMq.Interfaces;

namespace Notification.Consumers;

public class EmailConsumer(IChannel channel) : IMessageConsumer
{
    public IChannel Channel { get; } = channel;

    [ConsumerAttribute(
        queueName: QueueEnum.Email,
        autoAck: false,
        routingKey: RoutingKeyEnum.NotificationEmail
    )]
    public async Task ConsumeEmailMessage(string message)
    {
        Console.WriteLine($"Received email message: {message}");
        // Simulate processing time
        await Task.Delay(1000);
        Console.WriteLine("Email message processed.");
    }
}