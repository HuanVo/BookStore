using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BookStore.Commons
{
    public static class Services
    {
        /// <summary>
        /// Hashing to md5 for a string value
        /// </summary>
        /// <param name="text">text not hash</param>
        /// <returns>return a string value hashed</returns>
        public static string Md5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            //compute hash from the bytes of text
            md5.ComputeHash(Encoding.ASCII.GetBytes(text));
            //get hash result after compute it
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            foreach (byte t in result)
            {
                //change it into 2 hexadecimal digits
                strBuilder.Append(t.ToString("x2"));
            }
            return strBuilder.ToString();
        }
        /// <summary>
        /// Send email to user
        /// </summary>
        /// <param name="fromMail">email from</param>
        /// <param name="toMail">mail to</param>
        /// <param name="subject">mail subject</param>
        /// <param name="body">mail body</param>
        /// <param name="password">password of mail from</param>
        public static void SendMail(String fromMail, String toMail, String subject, String body, String password)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient(Constants.SMTP_SERVER);
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(fromMail);
                mail.To.Add(toMail);
                mail.Subject = subject;
                mail.Body = body;
                smtpServer.Port = 587;
                smtpServer.Credentials = new System.Net.NetworkCredential(fromMail, password);
                smtpServer.EnableSsl = true;
                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
#pragma warning disable CS0436 // Type conflicts with imported type
                ErrorLog.WriteLog(ex.Message);
#pragma warning restore CS0436 // Type conflicts with imported type
            }
        }

        #region Hashing  one to one
        /// <summary>
        /// Creates a key based on the specified character string.
        /// </summary>
        /// <param name="source">The source of the key.</param>
        /// <returns>The generated key.</returns>

        //public static string CreateKey(string source)
        //{
        //    MD5 md5Hasher = MD5.Create();
        //    byte[] hash = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(source));
        //    return BitConverter.ToString(hash).Replace("-", "");
        //}

        static readonly byte[] IV = new byte[] {
            0xE5, 0x52, 0xEA, 0x1E,
            0x48, 0x4F, 0x75, 0xBB,
            0x42, 0x81, 0x87, 0x89,
            0x9B, 0x76, 0xFE, 0xD4
        };

        private static byte[] CreateBytesKey(string key)
        {
            byte[] bytesKey = new byte[256 / 8];
            Encoding.UTF8.GetBytes(key).CopyTo(bytesKey, 0);
            return bytesKey;
        }

        private static void WriteCipher(string cipher, CryptoStream cs)
        {
            StringBuilder sb = new StringBuilder(cipher);
            string source = sb.Replace("-", "").Replace(",", "").ToString();

            for (int i = 0; i < source.Length; i += 2)
            {
                cs.WriteByte(Convert.ToByte(source.Substring(i, 2), 16));
            }
        }

        /// <summary>
        /// Encrypt the string.
        /// </summary>
        /// <param name="message">Message to encrypt.</param>
        /// <param name="key">The key used for encryption.</param>
        /// <returns>String of encrypted byte array.</returns>
        static public string EncryptMessage(string message, string key)
        {
            byte[] source = Encoding.UTF8.GetBytes(message);
            byte[] bytesKey = CreateBytesKey(key);
            Rijndael rijndael = Rijndael.Create();

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, rijndael.CreateEncryptor(bytesKey, IV), CryptoStreamMode.Write))
                {
                    cs.Write(source, 0, source.Length);
                    cs.FlushFinalBlock();
                    return BitConverter.ToString(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// Decrypt the string.
        /// </summary>
        /// <param name="cipher">Cipher text to decrypt.</param>
        /// <param name="key">The key used for decryption.</param>
        /// <returns>The decrypted character string.</returns>
        static public string DecryptMessage(string cipher, string key)
        {
            byte[] bytesKey = CreateBytesKey(key);
            Rijndael rijndael = Rijndael.Create();

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, rijndael.CreateDecryptor(bytesKey, IV), CryptoStreamMode.Write))
                {
                    WriteCipher(cipher, cs);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }

        #endregion
    }
}