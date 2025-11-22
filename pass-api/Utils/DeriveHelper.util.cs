using System.Security.Cryptography;

public class DeriveHelper
{
    public static byte[] RFC2898(string pass, byte[] salt, int bytes = 32, int iterations = 300000)
    {
        //Padrão RFC2898
        //Algoritmo pbkdf2
        var pbkdf2 = new Rfc2898DeriveBytes(
            password: pass,
            salt: salt,
            iterations: iterations,
            hashAlgorithm: HashAlgorithmName.SHA256
        );

        byte[] key = pbkdf2.GetBytes(bytes);

        return key;
    }
}