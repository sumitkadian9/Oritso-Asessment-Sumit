using System.Security.Cryptography;
using System.Text;
using TaskManagement.API.Models;

namespace TaskManagement.API.Common;

public static class PasswordHelper
{
    public static PasswordHashWithSalt GetPasswordWithSalt(string passwordText)
    {
        PasswordHashWithSalt passwordHashWithSalt = new PasswordHashWithSalt();
        passwordHashWithSalt.Salt = GenerateSalt();
        passwordHashWithSalt.Hash = ComputeHash(passwordText, passwordHashWithSalt.Salt);
        return passwordHashWithSalt;
    }

    public static bool IsValid(string passwordText, string userPasswordHash, string userPasswordSalt)
    {
        string passwordTextHash = ComputeHash(passwordText, userPasswordSalt);
        return passwordTextHash == userPasswordHash;
    }

    private static string ComputeHash(string passwordText, string salt)
    {
        using var sha256 = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(passwordText + salt);
        byte[] byteHash = sha256.ComputeHash(bytes);
        return BitConverter.ToString(byteHash).Replace("-", string.Empty).ToLower();
    }

    private static string GenerateSalt()
    {
        using var rng = RandomNumberGenerator.Create();
        byte[] byteSalt = new byte[16];
        rng.GetBytes(byteSalt);
        string salt = Convert.ToBase64String(byteSalt);
        return salt;
    }
}