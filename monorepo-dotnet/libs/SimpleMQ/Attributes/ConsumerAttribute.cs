using SimpleMq.Enums;

namespace SimpleMq.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class ConsumerAttribute(
    QueueEnum queueName,
    bool autoAck = false,
    RoutingKeyEnum routingKey = RoutingKeyEnum.None
) : Attribute
{
    public string QueueName { get; } = queueName.GetEnumMember();
    public string? RoutingKey { get; set; } = routingKey == RoutingKeyEnum.None ? null : routingKey.GetEnumMember();
    public bool AutoAck { get; } = autoAck;
}