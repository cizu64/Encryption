using System.Security.Cryptography;
using System.Text;

namespace Encryption;
public class AESCryptography
{
    public static string Encrypt(string plainText, string key)
    {

        using Aes aesAlg = Aes.Create();
        aesAlg.Key = GetKey(key);
        aesAlg.GenerateIV(); // Generating a random IV
        aesAlg.Mode = CipherMode.CBC;
        aesAlg.Padding = PaddingMode.PKCS7;

        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        using MemoryStream msEncrypt = new MemoryStream();
        msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
        {
            swEncrypt.Write(plainText);
        }

        return Convert.ToBase64String(msEncrypt.ToArray());

    }

    public static string Decrypt(string cipherText, string key)
    {

        byte[] fullCipher = Convert.FromBase64String(cipherText);

        using Aes aesAlg = Aes.Create();
        aesAlg.Key = GetKey(key);
        byte[] iv = new byte[aesAlg.BlockSize / 8];
        byte[] cipherBytes = new byte[fullCipher.Length - iv.Length];

        Array.Copy(fullCipher, iv, iv.Length);
        Array.Copy(fullCipher, iv.Length, cipherBytes, 0, cipherBytes.Length);

        aesAlg.IV = iv;
        aesAlg.Mode = CipherMode.CBC;
        aesAlg.Padding = PaddingMode.PKCS7;

        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
        {
            return srDecrypt.ReadToEnd();
        }
    }

    private static byte[] GetKey(string myKey)
    {
        using SHA1 sha = SHA1.Create();
        // Convert the input string to a byte array and compute the hash
        byte[] keyBytes = Encoding.UTF8.GetBytes(myKey);
        byte[] hashBytes = sha.ComputeHash(keyBytes);

        // Copy the first 16 bytes of the hash
        byte[] key = new byte[16];
        Array.Copy(hashBytes, key, key.Length);

        return key;
    }
}