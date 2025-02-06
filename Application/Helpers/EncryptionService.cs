using System.Security.Cryptography;
using System.Text;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Aes = System.Security.Cryptography.Aes;

namespace Application.Helpers;

public class EncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    private readonly byte[] _iv;
    public EncryptionService(IConfiguration configuration)
    {
        var encryptionSection = configuration.GetRequiredSection("Encryption");
        _key = Encoding.UTF8.GetBytes(encryptionSection["Key"] ?? throw new InvalidOperationException());
        _iv = Encoding.UTF8.GetBytes(encryptionSection["Iv"] ?? throw new InvalidOperationException());
    }
    public async Task<string> EncryptAsync(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) throw new ArgumentNullException(nameof(plainText));

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream();
        await using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

        var inputBytes = Encoding.UTF8.GetBytes(plainText);
        await cryptoStream.WriteAsync(inputBytes, 0, inputBytes.Length);
        await cryptoStream.FlushFinalBlockAsync();

        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public async Task<string> Decrypt(string encryptedText)
    {
        if (string.IsNullOrEmpty(encryptedText)) throw new ArgumentNullException(nameof(encryptedText));

        using var aes = Aes.Create();
        aes.Key = _key;
        aes.IV = _iv;

        using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var memoryStream = new MemoryStream(Convert.FromBase64String(encryptedText));
        await using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

        using var reader = new StreamReader(cryptoStream, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}