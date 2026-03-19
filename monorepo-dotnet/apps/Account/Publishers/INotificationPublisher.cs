using Account.Messages;

namespace Account.Publishers;

public interface INotificationPublisher
{
    Task PublishEmailAsync(EmailMessage message);
}