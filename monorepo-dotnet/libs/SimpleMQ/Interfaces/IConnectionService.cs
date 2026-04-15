using RabbitMQ.Client;

namespace SimpleMq.Interfaces;

public interface IConnectionService
{
    Task<IChannel> OpenChannel(CancellationToken cancellationToken = default);
    Task<IConnection> OpenConnection(CancellationToken cancellationToken = default);
}