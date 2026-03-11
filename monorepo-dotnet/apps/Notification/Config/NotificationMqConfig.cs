using RabbitMQ.Client;
using SimpleMq.Config;
using SimpleMq.Dtos;
using SimpleMq.Enums;

namespace Notification.Config;

public sealed class NotificationMqConfig : IExchangeConfig, IQueueConfig, IBindConfig
{
    public IReadOnlyCollection<ExchangeConfigDto> Exchanges { get; } =
    [
        new(ExchangeNameEnum.Notification.GetEnumMember(), ExchangeType.Direct, true, false)
    ];

    public IReadOnlyCollection<QueueConfigDto> Queues { get; } =
    [
        new(QueueEnum.Email.GetEnumMember(), true, false, false)
    ];

    public IReadOnlyCollection<BindConfigDto> Binds { get; } =
    [
        new(
            Queue: QueueEnum.Email,
            Exchange: ExchangeNameEnum.Notification,
            RoutingKey: RoutingKeyEnum.NotificationEmail.GetEnumMember()
        )
    ];
}