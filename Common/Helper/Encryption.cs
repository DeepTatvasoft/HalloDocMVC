using System.Security.Cryptography;
using System.Text;

namespace Common.Helper
{
    public class EncryptDecryptHelper
    {
        public const string secretKey = "ORM@714rGK#zO>6H61Sdt<ST[yx\\38!@GW£|Vx>lg";

        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return string.Empty;
            }

            try
            {
                // Get the bytes of the string
                byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(secretKey);

                // Hash the password with SHA256
                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);

                return GetHexString(bytesEncrypted);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string Decrypt(string encryptedText)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                return string.Empty;
            }

            try
            {
                // Get the bytes of the string
                byte[] bytesToBeDecrypted = GetBytes(encryptedText);
                byte[] passwordBytes = Encoding.UTF8.GetBytes(secretKey);

                passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                byte[] bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);

                if (bytesDecrypted == null)
                {
                    return string.Empty;
                }

                return Encoding.UTF8.GetString(bytesDecrypted);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[]? encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream ms = new())
            {
                using (RijndaelManaged aes = new())
                {
                    Rfc2898DeriveBytes key = new(passwordBytes, saltBytes, 1000);
                    aes.KeySize = 256;
                    aes.BlockSize = 128;
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);
                    aes.Mode = CipherMode.CBC;

                    using (CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            try
            {
                byte[]? decryptedBytes = null;

                // Set your salt here, change it to meet your flavor:
                // The salt bytes must be at least 8 bytes.
                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
                using (MemoryStream ms = new())
                {
                    using (RijndaelManaged aes = new())
                    {
                        Rfc2898DeriveBytes key = new(passwordBytes, saltBytes, 1000);
                        aes.KeySize = 256;
                        aes.BlockSize = 128;
                        aes.Key = key.GetBytes(aes.KeySize / 8);
                        aes.IV = key.GetBytes(aes.BlockSize / 8);
                        aes.Mode = CipherMode.CBC;

                        using (CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }

                        decryptedBytes = ms.ToArray();
                    }
                }

                return decryptedBytes;
            }
            catch (Exception ex)
            {
                return null!;
            }
        }

        private static string GetHexString(byte[] bytes)
        {
            StringBuilder sb = new();
            foreach (byte b in bytes)
            {
                string hex = b.ToString("x2");
                sb.Append(hex);
            }

            return sb.ToString();
        }

        private static byte[] GetBytes(string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }
    }

}
