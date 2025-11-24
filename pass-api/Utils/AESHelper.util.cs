using System.Security.Cryptography;

namespace PasswordManager.Utils;

public class AESHelper
{
    public static string Encrypt(string secretKey, string pass)
    {
        if (string.IsNullOrEmpty(secretKey))
            throw new ArgumentNullException(nameof(secretKey), "A chave secreta não pode ser nula ou vazia.");
        if (string.IsNullOrEmpty(pass))
            throw new ArgumentNullException(nameof(pass), "A senha não pode ser nula ou vazia.");


        using var aes = Aes.Create();
        aes.GenerateIV();
        var iv = aes.IV;

        var secretKeyDerived = DeriveHelper.RFC2898(secretKey, iv);
        aes.Key = secretKeyDerived;
        
        using var encryptor = aes.CreateEncryptor();
        using var memoryStream = new MemoryStream();
        memoryStream.Write(iv, 0, iv.Length); // Anexa o IV no início

        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        using var streamWriter = new StreamWriter(cryptoStream);
        streamWriter.Write(pass);
        streamWriter.Flush();
        cryptoStream.FlushFinalBlock();

        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public static string Decrypt(string secretKey, string encryptedText){
        var aes = Aes.Create();

        var encryptedBytes = Convert.FromBase64String(encryptedText);
        var iv = new byte[16];
        Array.Copy(encryptedBytes, 0, iv, 0, iv.Length);
        aes.IV = iv;

        var secretKeyDerived = DeriveHelper.RFC2898(secretKey, iv);
        aes.Key = secretKeyDerived;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream(encryptedBytes, iv.Length, encryptedBytes.Length - iv.Length);
        using var decryptorStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        using var streamReader = new StreamReader(decryptorStream);
        return streamReader.ReadToEnd();
    }

}