using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TCPCLient.Services
{
    public static class EncryptionService
    {
        // Must be 32 bytes (256-bit key)
        private static readonly string Key = "12345678901234567890123456789012";

        // Must be 16 bytes
        private static readonly string IV = "1234567890123456";

        public static string Encrypt(string plainText)
        {
            using Aes aes = Aes.Create();

            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = Encoding.UTF8.GetBytes(IV);

            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using StreamWriter writer = new(cs);

            writer.Write(plainText);
            writer.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string Decrypt(string cipherText)
        {
            using Aes aes = Aes.Create();

            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = Encoding.UTF8.GetBytes(IV);

            byte[] buffer = Convert.FromBase64String(cipherText);

            using MemoryStream ms = new(buffer);
            using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using StreamReader reader = new(cs);

            return reader.ReadToEnd();
        }
    }
}
