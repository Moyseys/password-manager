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
        var declaredDlqExchanges = new HashSet<string>(StringComparer.Ordinal);

        foreach (var exchange in exchangeConfig.Exchanges)
        {
            await channel.ExchangeDeclareAsync(
                exchange: exchange.Name,
                type: exchange.Type,
                durable: exchange.Durable,
                autoDelete: exchange.AutoDelete
            );

            var dlqExchangeName = GetDlqName(exchange.Name);
            if (declaredDlqExchanges.Add(dlqExchangeName))
            {
                await channel.ExchangeDeclareAsync(
                    exchange: dlqExchangeName,
                    type: exchange.Type,
                    durable: exchange.Durable,
                    autoDelete: exchange.AutoDelete
                );
            }
        }
    }

    public async Task SetupQueues(IChannel channel, IQueueConfig queueConfig)
    {
        foreach (var queue in queueConfig.Queues)
        {
            var sourceExchangeName = ResolveSourceExchangeName(queue.Name);
            var dlqExchangeName = GetDlqName(sourceExchangeName);
            var dlqQueueName = GetDlqName(queue.Name);
            var arguments = new Dictionary<string, object?>
            {
                ["x-dead-letter-exchange"] = dlqExchangeName,
                ["x-dead-letter-routing-key"] = dlqQueueName
            };

            await channel.QueueDeclareAsync(
                queue: queue.Name,
                durable: queue.Durable,//mantem as filas ao reiniciar
                exclusive: queue.Exclusive, //Outras conexões podem usaar essa fila, util para filas temporarias,
                autoDelete: queue.AutoDelete,//Deleta quaando não há consumidor
                arguments: arguments
            );

            await channel.QueueDeclareAsync(
                queue: dlqQueueName,
                durable: queue.Durable,
                exclusive: false,
                autoDelete: false
            );

            await channel.QueueBindAsync(
                queue: dlqQueueName,
                exchange: dlqExchangeName,
                routingKey: dlqQueueName
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

    private string ResolveSourceExchangeName(string queueName)
    {
        var bind = _bindConfig.Binds.FirstOrDefault(x => x.Queue.GetEnumMember() == queueName)
            ?? throw new InvalidOperationException($"No binding found for queue: {queueName}");
        return bind.Exchange.GetEnumMember();

    }

    private static string GetDlqName(string value) => $"{value}.dlq";
}
