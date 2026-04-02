using SharedDto.Messages;

namespace Account.Publishers;

public interface INotificationPublisher
{
    Task PublishEmailAsync(EmailMessage message);
}