using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using SimpleMq.Config;
using SimpleMq.Interfaces;
using SimpleMq.Options;
using SimpleMq.Services;

namespace SimpleMq.Extensions;

public static class SimpleMQExtensions
{
    public static IServiceCollection AddSimpleMQ(
        this IServiceCollection services,
        IConfiguration configuration,
        IExchangeConfig exchangeConfig,
        IQueueConfig queueConfig,
        IBindConfig bindConfig
    )
    {
        services.AddOptions<MessageBrokerConnectionOptions>()
            .Bind(configuration.GetSection(nameof(MessageBrokerConnectionOptions)));
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerConnectionOptions>>().Value);

        services.AddSingleton(exchangeConfig);
        services.AddSingleton(queueConfig);
        services.AddSingleton(bindConfig);

        services.AddSingleton<ISetupMQService, SetupMQService>();
        services.AddSingleton<IConnectionService, ConnectionService>();

        services.AddHostedService<MQBootstrapService>();

        return services;
    }
}