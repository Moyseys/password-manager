using SimpleMq.Config;
using RabbitMQ.Client;
using SimpleMq.Interfaces;

namespace SimpleMq.Services;

public class SetupMQService(
    IExchangeConfig exchangeConfig,
    IQueueConfig queueConfig,
    IBindConfig bindConfig) : ISetupMQService
{
    private readonly IExchangeConfig _exchangeConfig = exchangeConfig;
    private readonly IQueueConfig _queueConfig = queueConfig;
    private readonly IBindConfig _bindConfig = bindConfig;

    public async Task SetupBroker(IConnection connection)
    {
        await using var channel = await connection.CreateChannelAsync();

        await channel.BasicQosAsync(
            prefetchSize: 0, //Sem limite de tamanho para mensagens
            prefetchCount: 1, //Processa uma mensagem por vez
            global: false //Configuração específica para este canal
        );

        await SetupExchanges(channel, _exchangeConfig);
        await SetupQueues(channel, _queueConfig);
        await SetupBindings(channel, _bindConfig);
    }

    public async Task SetupExchanges(IChannel channel, IExchangeConfig exchangeConfig)
    {
        foreach (var exchange in exchangeConfig.Exchanges)
        {
            await channel.ExchangeDeclareAsync(
                exchange: exchange.Name,
                type: exchange.Type,
                durable: exchange.Durable,
                autoDelete: exchange.AutoDelete
            );
        }
    }

    public async Task SetupQueues(IChannel channel, IQueueConfig queueConfig)
    {
        foreach (var queue in queueConfig.Queues)
        {
            await channel.QueueDeclareAsync(
                queue: queue.Name,
                durable: queue.Durable,//mantem as filas ao reiniciar
                exclusive: queue.Exclusive, //Outras conexões podem usaar essa fila, util para filas temporarias,
                autoDelete: queue.AutoDelete//Deleta quaando não há consumidor
            );
        }
    }

    public async Task SetupBindings(IChannel channel, IBindConfig bindConfig)
    {
        foreach (var bind in bindConfig.Binds)
        {
            await channel.QueueBindAsync(
                queue: bind.Queue.GetEnumMember(),
                exchange: bind.Exchange.GetEnumMember(),
                routingKey: bind.RoutingKey
            );
        }
    }
}