using System.Runtime.Serialization;

namespace SimpleMq.Enums;

public enum RoutingKeyEnum
{
    None = 0,

    [EnumMember(Value = "notification.email")]
    NotificationEmail
}