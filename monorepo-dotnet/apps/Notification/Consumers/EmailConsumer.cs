using Microsoft.Extensions.Logging;
using Notification.Interfaces;
using SharedDto.Messages;
using SimpleMq.Attributes;
using SimpleMq.Enums;
using SimpleMq.Interfaces;

namespace Notification.Consumers;

public class EmailConsumer(ILogger<EmailConsumer> logger, IEmailService emailService) : IMessageConsumer
{
    private IEmailService EmailService { get; } = emailService;
    public ILogger<EmailConsumer> Logger { get; } = logger;

    [ConsumerAttribute(
        queueName: QueueEnum.Email,
        autoAck: false,
        routingKey: RoutingKeyEnum.NotificationEmail
    )]
    public async Task ConsumeEmailMessage(EmailMessage message)
    {
        Logger.LogInformation($"Received email message: {message}");
        await EmailService.SendEmailAsync(message);
    }
}