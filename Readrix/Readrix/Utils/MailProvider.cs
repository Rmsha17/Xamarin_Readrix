using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace Readrix.Utils
{
    class MailProvider
    {
        public static bool SenttoMail(string receiver, string subject, string body)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("readrix28@gmail.com");
                message.To.Add(new MailAddress(receiver));
                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = body;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("readrix28@gmail.com", "ejinwqithimbfxxs");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
