namespace SimpleMq.Dtos;

public sealed record ConsumerRegistration(
    Type HandlerType,
    string MethodName,
    string QueueName,
    bool AutoAck,
    string? RoutingKey,
    Type? PayloadType,
    Func<object, object?, Task> InvokeAsync
);