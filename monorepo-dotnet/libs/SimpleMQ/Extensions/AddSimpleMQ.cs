using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
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

        var consumerTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a =>
            {
                try { return a.GetTypes(); }
                catch (ReflectionTypeLoadException ex) { return ex.Types.OfType<Type>(); }
            })
            .Where(t => typeof(IMessageConsumer).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var consumerType in consumerTypes)
        {
            services.AddScoped(consumerType);
        }

        services.AddHostedService<MQBootstrapService>();

        return services;
    }
}