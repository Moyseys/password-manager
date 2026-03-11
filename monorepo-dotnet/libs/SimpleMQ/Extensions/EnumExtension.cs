using System.Reflection;
using System.Runtime.Serialization;

public static class EnumExtension
{
    public static string GetEnumMember(this Enum queue)
    {
        var member = queue.GetType().GetMember(queue.ToString()).FirstOrDefault();
        var enumMember = member?.GetCustomAttribute<EnumMemberAttribute>();

        return enumMember?.Value ?? queue.ToString();
    }
}