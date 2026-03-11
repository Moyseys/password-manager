namespace SimpleMq.Config;

public interface IBindConfig
{
    IReadOnlyCollection<BindConfigDto> Binds { get; }
}