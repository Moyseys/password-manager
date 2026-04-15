namespace SimpleMq.Options;

public sealed class MessageBrokerConnectionOptions
{
    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public string UserName { get; set; } = "guest";
    public string Password { get; set; } = "guest";
    public bool AutomaticRecoveryEnabled { get; set; } = true;
}