using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Helpers
{
    public class Message
    {
        public void Send(string reciever,string subject, string message,string messageType)
        {
            if (messageType == "email")
                SendEmail(reciever,subject,message);
        }

        public void SendEmail(string reciever,string subject, string message)
        {
           // SmtpClient client = new SmtpClient("https://ghanongostar.com/webmail");

            SmtpClient client = new SmtpClient("https://webmail.ghanongostar.com");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = "185.129.169.21";
            client.Port = 25;

            //If you need to authenticate
            client.Credentials = new NetworkCredential("support@ghanongostar.com", "Zeqq17#8f#@bF85");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("support@ghanongostar.com");
            mailMessage.To.Add(reciever);
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            
            mailMessage.IsBodyHtml = true;

            client.Send(mailMessage);
        }
    }
}