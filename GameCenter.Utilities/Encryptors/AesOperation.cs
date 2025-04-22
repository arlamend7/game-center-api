using SGTC.Utilities.Encryptors.Interfaces;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SGTC.Utilities.Encryptors
{
    public sealed class AesOperation : IAesOperation
    {
        private readonly byte[] key;
        private readonly byte[] iv = new byte[16];

        public AesOperation()
        {

        }
        public AesOperation(string key)
        {
            this.key = Encoding.UTF8.GetBytes(key);
        }
        public string Encrypt<T>(T entity)
        {
            return Encrypt(JsonSerializer.Serialize(entity));
        }
        public string Encrypt(string plainText)
        {
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }
                    array = memoryStream.ToArray();
                }

            }

            return Convert.ToBase64String(array);
        }

        public T Decrypt<T>(string cipherText)
        {
            return JsonSerializer.Deserialize<T>(Decrypt(cipherText));
        }
        public string Decrypt(string cipherText)
        {
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {

                aes.Key = key;
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                using (StreamReader streamReader = new StreamReader(cryptoStream))
                    return streamReader.ReadToEnd();
            }
        }
    }
}
