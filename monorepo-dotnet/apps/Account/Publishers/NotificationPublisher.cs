using SharedDto.Messages;
using SimpleMq.Enums;
using SimpleMq.Interfaces;
using SimpleMq.Services;

namespace Account.Publishers;

public class NotificationPublisher(IConnectionService connectionService) : BasePublisherService(connectionService)
    , INotificationPublisher
{
    public async Task PublishEmailAsync(EmailMessage message)
    {
        await PublishMessageAsync(
            exchange: ExchangeNameEnum.Notification.GetEnumMember(),
            routingKey: RoutingKeyEnum.NotificationEmail.GetEnumMember(),
            message: message
        );
    }
}