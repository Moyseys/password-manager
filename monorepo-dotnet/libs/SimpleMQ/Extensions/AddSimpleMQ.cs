using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;
using SimpleMq.Config;
using SimpleMq.Dtos;
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
        IBindConfig bindConfig,
        params Assembly[] consumerAssemblies
    )
    {
        services.AddOptions<MessageBrokerConnectionOptions>()
            .Bind(configuration.GetSection(nameof(MessageBrokerConnectionOptions)))
            .ValidateOnStart();
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerConnectionOptions>>().Value);

        services.AddSingleton(exchangeConfig);
        services.AddSingleton(queueConfig);
        services.AddSingleton(bindConfig);

        services.AddSingleton<ISetupMQService, SetupMQService>();
        services.AddSingleton<IConnectionService, ConnectionService>();
        services.AddSingleton<ConsumerMessageDispatcher>();

        var assembliesToScan = consumerAssemblies.Length > 0
            ? consumerAssemblies
            : AppDomain.CurrentDomain.GetAssemblies();

        var buildResult = ConsumerRegistrationBuilder.Build(assembliesToScan);

        foreach (var consumerType in buildResult.ConsumerTypes)
        {
            services.AddScoped(consumerType);
        }

        services.AddSingleton(buildResult.Registrations);

        services.AddHostedService<MQBootstrapService>();

        return services;
    }
}