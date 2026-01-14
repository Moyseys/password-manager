using System.Security.Cryptography;
using System.Text;

namespace Core.Utils;

public class HashHashHelper{
    public static string ComputeSha256Hash(string v) {
        using SHA256 sha256Hash = SHA256.Create();
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(v));

        var stringBuilder = new StringBuilder();
        foreach (var b in bytes)
        {
            stringBuilder.Append(b.ToString("x2"));
        }
        return stringBuilder.ToString();
    }
}