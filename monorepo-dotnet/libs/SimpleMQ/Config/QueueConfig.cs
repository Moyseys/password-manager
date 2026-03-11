using SimpleMq.Dtos;

namespace SimpleMq.Config;

public interface IQueueConfig
{
    IReadOnlyCollection<QueueConfigDto> Queues { get; }
}