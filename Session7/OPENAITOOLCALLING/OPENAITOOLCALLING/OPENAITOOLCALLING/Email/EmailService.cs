using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
namespace OPENAITOOLCALLING.Email
{
    public class EmailService
    {
        public static string SendEmail(string to, string text)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Devesh", "devesh.akgec@gmail.com"));
            message.To.Add(new MailboxAddress("Recipient", to));

            message.Subject = "Email from HR Department";

            message.Body = new TextPart("plain")
            {
                Text = text
            };

            using var client = new SmtpClient();

            client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

            client.Authenticate(
                "devesh.akgec@gmail.com",
                "tswm acwj mwvs ruyy");

            client.Send(message);

            client.Disconnect(true);

            Console.WriteLine("Email Sent Successfully");

            return "Email Sent";
        }
    }
}
