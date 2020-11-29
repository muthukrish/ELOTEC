namespace ELOTEC.Infrastructure.Security
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class Cryptography
    {
        // <summary>
        /// The encryption key
        /// </summary>
        private static readonly string _encryptionKey = "EL0TECH$2019";
        /// <summary>
        /// The salt
        /// </summary>
        private static readonly byte[] _salt = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };

        /// <summary>
        /// Encrypts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string Encrypt(string value)
        {
            byte[] textByte = Encoding.Unicode.GetBytes(value);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_encryptionKey, _salt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream csStream = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        csStream.Write(textByte, 0, textByte.Length);
                        csStream.Close();
                    }

                    value = Convert.ToBase64String(ms.ToArray());
                }
            }

            return value;
        }

        /// <summary>
        /// Decrypts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string Decrypt(string value)
        {
            byte[] textByte = Convert.FromBase64String(value);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(_encryptionKey, _salt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(textByte, 0, textByte.Length);
                        cs.Close();
                    }

                    value = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return value;
        }
    }
}
