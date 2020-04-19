namespace AssistanceRequestApp.Common
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="EncryptDecrypt" />.
    /// </summary>
    public class EncryptDecrypt
    {
        /// <summary>
        /// Defines the initVector.
        /// </summary>
        private const string initVector = "tu89geji340t89u2";

        /// <summary>
        /// Defines the keysize.
        /// </summary>
        private const int keysize = 256;

        /// <summary>
        /// The Encrypt.
        /// </summary>
        /// <param name="Text">The Text<see cref="string"/>.</param>
        /// <param name="Key">The Key<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Encrypt(string Text, string Key)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(Text);
            PasswordDeriveBytes password = new PasswordDeriveBytes(Key, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged
            {
                Mode = CipherMode.CBC
            };
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();
            byte[] Encrypted = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            return Convert.ToBase64String(Encrypted);
        }

        /// <summary>
        /// The Decrypt.
        /// </summary>
        /// <param name="EncryptedText">The EncryptedText<see cref="string"/>.</param>
        /// <param name="Key">The Key<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Decrypt(string EncryptedText, string Key)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] DeEncryptedText = Convert.FromBase64String(EncryptedText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(Key, null);
            byte[] keyBytes = password.GetBytes(keysize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged
            {
                Mode = CipherMode.CBC
            };
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(DeEncryptedText);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[DeEncryptedText.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }
    }
}
