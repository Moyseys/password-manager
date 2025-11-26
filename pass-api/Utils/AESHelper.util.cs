using System.Security.Cryptography;

namespace PasswordManager.Utils;

public class AESHelper
{
    public static byte[] Encrypt(byte[] secretKey, byte[] encryptedContent)
    {
        using var aes = Aes.Create();
        aes.GenerateIV();
        var iv = aes.IV;

        aes.Key = secretKey;
        
        using var encryptor = aes.CreateEncryptor();
        using var memoryStream = new MemoryStream();
        memoryStream.Write(iv, 0, iv.Length); // Anexa o IV no início

        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(encryptedContent, 0, encryptedContent.Length);
        cryptoStream.FlushFinalBlock();

        return memoryStream.ToArray();
    }

    public static byte[] Decrypt(byte[] secretKey, byte[] encryptedContent){
        using var aes = Aes.Create();

        var iv = new byte[16];
        Array.Copy(encryptedContent, 0, iv, 0, iv.Length);
        aes.IV = iv;

        aes.Key = secretKey;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream(encryptedContent, iv.Length, encryptedContent.Length - iv.Length);
        using var decryptorStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var outStream = new MemoryStream();
        decryptorStream.CopyTo(outStream);
        return outStream.ToArray();
    }

}