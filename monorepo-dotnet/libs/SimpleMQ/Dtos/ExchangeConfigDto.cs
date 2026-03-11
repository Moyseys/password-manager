namespace SimpleMq.Dtos;

public record ExchangeConfigDto(
    string Name,
    string Type,
    bool Durable,
    bool AutoDelete
);