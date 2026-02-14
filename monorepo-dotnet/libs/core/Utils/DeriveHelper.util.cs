using System.Security.Cryptography;

namespace Core.Utils;

public class DeriveHelper
{
    public static byte[] RFC2898(string pass, byte[] salt, int bytes = 32, int iterations = 300000)
    {
        // RFC2898 Standard
        // pbkdf2 Algorithm
        byte[] key = Rfc2898DeriveBytes.Pbkdf2(
            pass,
            salt,
            iterations,
            HashAlgorithmName.SHA256,
            bytes
        );

        return key;
    }
}