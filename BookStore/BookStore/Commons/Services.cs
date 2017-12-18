using System;
using System.Collections.Generic;
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
                SmtpClient smtpServer = new SmtpClient(Constancs.SMTP_SERVER);
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

    }
}