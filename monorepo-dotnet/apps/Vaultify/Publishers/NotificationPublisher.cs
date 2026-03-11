using SimpleMq.Enums;
using SimpleMq.Interfaces;
using SimpleMq.Services;
using Vaultify.Messages;

namespace Vaultify.Publishers;

public class NotificationPublisher(IConnectionService connectionService) : BasePublisherService(connectionService)
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