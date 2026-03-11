namespace SimpleMq.Options;

public sealed class MessageBrokerConnectionOptions
{
    public string HostName { get; set; }
    public int Port { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public bool AutomaticRecoveryEnabled { get; set; }
}