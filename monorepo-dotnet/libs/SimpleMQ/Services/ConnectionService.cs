using RabbitMQ.Client;
using SimpleMq.Interfaces;
using SimpleMq.Options;

namespace SimpleMq.Services;

public class ConnectionService : IConnectionService
{
    private readonly ConnectionFactory _factory;
    public IConnection Connection { get; }

    public ConnectionService(MessageBrokerConnectionOptions options)
    {
        _factory = new ConnectionFactory()
        {
            HostName = options.HostName,
            Port = options.Port,
            UserName = options.UserName,
            Password = options.Password,
            AutomaticRecoveryEnabled = options.AutomaticRecoveryEnabled,
        };

        Connection = OpenConnection().GetAwaiter().GetResult();
    }

    public async Task<IConnection> OpenConnection()
    {
        return await _factory.CreateConnectionAsync();
    }

    public async Task<IChannel> OpenChannel()
    {
        return await Connection.CreateChannelAsync();
    }
}