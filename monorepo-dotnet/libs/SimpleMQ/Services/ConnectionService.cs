using RabbitMQ.Client;
using SimpleMq.Interfaces;
using SimpleMq.Options;

namespace SimpleMq.Services;

public sealed class ConnectionService : IConnectionService, IAsyncDisposable
{
    private readonly ConnectionFactory _factory;
    private readonly SemaphoreSlim _connectionLock = new(1, 1);
    private IConnection? _connection;
    private bool _disposed;

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
    }

    public async Task<IConnection> OpenConnection(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        if (_connection is { IsOpen: true })
        {
            return _connection;
        }

        await _connectionLock.WaitAsync(cancellationToken);
        try
        {
            ThrowIfDisposed();

            if (_connection is { IsOpen: true })
            {
                return _connection;
            }

            if (_connection is not null)
            {
                try { await _connection.CloseAsync(); }
                catch { }
                _connection.Dispose();
                _connection = null;
            }

            _connection = await _factory.CreateConnectionAsync(cancellationToken);
            return _connection;
        }
        finally
        {
            _connectionLock.Release();
        }
    }

    public async Task<IChannel> OpenChannel(CancellationToken cancellationToken = default)
    {
        ThrowIfDisposed();

        var connection = await OpenConnection(cancellationToken);
        return await connection.CreateChannelAsync(cancellationToken: cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await _connectionLock.WaitAsync();
        try
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            if (_connection is not null)
            {
                try { await _connection.CloseAsync(); }
                catch { }

                _connection.Dispose();
                _connection = null;
            }
        }
        finally
        {
            _connectionLock.Release();
            _connectionLock.Dispose();
        }
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(ConnectionService));
        }
    }
}