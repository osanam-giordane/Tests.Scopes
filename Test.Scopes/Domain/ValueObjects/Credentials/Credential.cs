using System.Security.Cryptography;
using Test.Scopes.Abstractions.Domain.ValueObjects;
using Test.Scopes.Domain.Aggregates.Customers;

namespace Test.Scopes.Domain.ValueObjects.Credentials;

public record Credential(string Login) : ValueObject
{
    public Credential(string login, string password)
        : this(login)
    {
        using var aes = Aes.Create();
        ICryptoTransform cryptoTransform = aes.CreateEncryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream();
        using (var cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
        {
            using StreamWriter streamWriter = new StreamWriter(cryptoStream);
            streamWriter.Write(password);
        }
        Password = memoryStream.ToArray();
    }

    public Credential(string login, byte[] password)
        : this(login)
        => Password = password;

    public byte[] Password { get; init; }

    protected override bool Validate()
        => OnValidate<CredentialValidator, Credential>();
}