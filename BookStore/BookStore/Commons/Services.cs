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
        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits
                //for each byte
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        public static void SendMail(String fromMail, String toMail, String subject, String body, String password )
        {
            try
            {
                MailMessage mail = new MailMessage();
               
                SmtpClient SmtpServer = new SmtpClient(Constancs.SMTP_SERVER);
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(fromMail);
                mail.To.Add(toMail);
                mail.Subject = subject;
                mail.Body = body;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(fromMail, password);
                SmtpServer.EnableSsl = true;
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                ErrorLog.WriteLog(ex.Message);
            }
        }

    }
}