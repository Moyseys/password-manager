using SimpleMq.Enums;

namespace SimpleMq.Config;

public record BindConfigDto(
    QueueEnum Queue,
    ExchangeNameEnum Exchange,
    string RoutingKey
);