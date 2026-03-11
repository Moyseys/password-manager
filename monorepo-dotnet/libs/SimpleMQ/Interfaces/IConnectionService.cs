using RabbitMQ.Client;

namespace SimpleMq.Interfaces;

public interface IConnectionService
{
    public IConnection Connection { get; }

    Task<IChannel> OpenChannel();
    Task<IConnection> OpenConnection();
}