namespace SimpleMq.Dtos;

public record QueueConfigDto(
    string Name,
    bool Durable,
    bool Exclusive,
    bool AutoDelete
);