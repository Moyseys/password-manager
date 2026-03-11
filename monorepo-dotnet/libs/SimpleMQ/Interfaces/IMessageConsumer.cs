using RabbitMQ.Client;

namespace SimpleMq.Interfaces;

public interface IMessageConsumer
{
    IChannel Channel { get; }
}
