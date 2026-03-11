using SimpleMq.Dtos;

namespace SimpleMq.Config;

public interface IExchangeConfig
{
    IReadOnlyCollection<ExchangeConfigDto> Exchanges { get; }
}
