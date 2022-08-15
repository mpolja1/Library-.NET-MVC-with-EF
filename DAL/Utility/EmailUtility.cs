using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Utility
{
    public class EmailUtility
    {
        private static readonly string SMTP_SERVER = "smtp.gmail.com";
        private static readonly int PORT = 587;
        private static readonly string SENDER_NAME = "MyLibrary";
        private static readonly string FROM = "libraryapp99@gmail.com";
        private static readonly string EMAIL_APP_PASSWORD = "yvposhcvxkefbmjv";



        public static void SendMail(string To, string Subject, string Body, string[] FileNames)
        {
            var smtpClient = new SmtpClient(SMTP_SERVER, PORT)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                EnableSsl = true
            };
            smtpClient.Credentials = new NetworkCredential(FROM, EMAIL_APP_PASSWORD); 

            var message = new MailMessage
            {
                From = new MailAddress(FROM, SENDER_NAME),
                Subject = Subject,
                Body = Body,
            };
            message.To.Add(new MailAddress(To, To));
            smtpClient.Send(message);
        }

    }
}

