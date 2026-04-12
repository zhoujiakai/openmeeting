using System.Security.Cryptography;
using System.Text;

namespace MeetingSystem.Common.Helper;

/// <summary>
/// AES 对称加密工具类，用于密码的加密和解密
/// </summary>
public static class AesHelper
{
    private static readonly string Key = "MeetingSystem2026OpenMeetingSecretKey";
    private static readonly byte[] KeyBytes = SHA256.HashData(Encoding.UTF8.GetBytes(Key));
    private static readonly byte[] Iv = MD5.HashData(Encoding.UTF8.GetBytes(Key));

    /// <summary>
    /// AES 加密
    /// </summary>
    /// <param name="plainText">明文</param>
    /// <returns>Base64 编码的密文</returns>
    public static string Encrypt(string plainText)
    {
        if (string.IsNullOrEmpty(plainText)) return plainText;

        using var aes = Aes.Create();
        aes.Key = KeyBytes;
        aes.IV = Iv;

        using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }
        return Convert.ToBase64String(ms.ToArray());
    }

    /// <summary>
    /// AES 解密。如果解密失败（如旧数据非 AES 加密），返回原始值。
    /// </summary>
    /// <param name="cipherText">Base64 编码的密文</param>
    /// <returns>明文，解密失败时返回原始值</returns>
    public static string Decrypt(string cipherText)
    {
        if (string.IsNullOrEmpty(cipherText)) return cipherText;

        try
        {
            using var aes = Aes.Create();
            aes.Key = KeyBytes;
            aes.IV = Iv;

            var buffer = Convert.FromBase64String(cipherText);
            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(buffer);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }
        catch
        {
            return cipherText;
        }
    }

    /// <summary>
    /// 判断字符串是否为 AES 加密后的 Base64 密文
    /// </summary>
    public static bool IsEncrypted(string text)
    {
        if (string.IsNullOrEmpty(text)) return false;

        try
        {
            var buffer = Convert.FromBase64String(text);
            using var aes = Aes.Create();
            aes.Key = KeyBytes;
            aes.IV = Iv;
            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(buffer);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            sr.ReadToEnd();
            return true;
        }
        catch
        {
            return false;
        }
    }
}
