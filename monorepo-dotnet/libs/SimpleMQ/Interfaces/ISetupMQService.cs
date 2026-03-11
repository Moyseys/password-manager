using RabbitMQ.Client;
using SimpleMq.Config;

namespace SimpleMq.Interfaces;

public interface ISetupMQService
{
    Task SetupBroker(IConnection connection);
    Task SetupExchanges(IChannel channel, IExchangeConfig exchangeConfig);
    Task SetupQueues(IChannel channel, IQueueConfig queueConfig);
    Task SetupBindings(IChannel channel, IBindConfig bindConfig);
}