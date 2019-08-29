using System.Text;
using System.Security.Cryptography;
using System.Security;


public static class UniqueHash
{

    public static string HashString(string text)
    {
        // System.Console.WriteLine(GetString(GetBytes(text)));
        using (SHA512 sha512 = SHA512.Create())
        {
            return GetHashString(sha512.ComputeHash(GetBytes(text)));
        }
    }

    public static string HashString(HashAlgorithm algorithm, string text)
    {
        string hashedString = GetHashString(algorithm.ComputeHash(GetBytes(text)));
        algorithm.Dispose();
        return hashedString;

    }

    public static SecureString SecureHashString(string text)
    {
        // System.Console.WriteLine(GetString(GetBytes(text)));
        using (SHA512 sha512 = SHA512.Create())
        {
            SecureString secureHash = SecureGetHashString(sha512.ComputeHash(GetBytes(text)));
            return secureHash;
        }
    }

    public static SecureString SecureHashString(HashAlgorithm algorithm, string text)
    {
            SecureString secureHash = SecureGetHashString(algorithm.ComputeHash(GetBytes(text)));
            algorithm.Dispose();
            return secureHash;
    }

    private static byte[] GetBytes(string text)
    {

        byte[] returnBytes = new byte[text.Length];

        for (int i = 0; i < text.Length; i++)
        {
            byte karakter = ((byte)text[i]);
            int modositott = (karakter);
            returnBytes[i] = (byte)modositott;
           // System.Console.WriteLine(modositott);
        }

        return returnBytes;

    }

    private static string GetHashString(byte[] bytes)
    {
        StringBuilder returnText = new StringBuilder();

        foreach (var bit in bytes)
        {
            returnText.Append(bit.ToString("x2"));
        }

        return returnText.ToString();

    }

     private static SecureString SecureGetHashString(byte[] bytes)
    {
        SecureString returnText = new SecureString();

        foreach (var bit in bytes)
        {
            returnText.AppendChar(bit.ToString("x2")[0]);
        }

        return returnText;

    }

    private static string GetString(byte[] bytes)
    {

        StringBuilder returnText = new StringBuilder();

        for (int i = 0; i < bytes.Length; i++)
        {
            char karakter = (char)(bytes[i] + 4);
            returnText.Append(karakter);
        }

        return returnText.ToString();

    }
}
