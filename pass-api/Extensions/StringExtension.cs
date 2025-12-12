namespace PasswordManager.Extensions;

using System.Text.RegularExpressions;

public static class StringExtension
{
    public static string ToSnakeCase(this string name)
    {
        if(String.IsNullOrEmpty(name)) return name;

        var result = name.Trim();
        
        result = Regex.Replace(result, @"\s+", "_");
        
        result = Regex.Replace(result, @"([a-z0-9])([A-Z])", "$1_$2");
        
        result = Regex.Replace(result, @"([A-Z])([A-Z][a-z])", "$1_$2");
        
        result = Regex.Replace(result, @"_+", "_");
        
        result = result.Trim('_');
        
        return result.ToLower();
    }
}