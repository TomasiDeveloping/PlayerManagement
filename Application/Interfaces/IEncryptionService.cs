namespace Application.Interfaces;

public interface IEncryptionService
{
    Task<string> EncryptAsync(string plainText);

    Task<string> Decrypt(string encryptedText);
}