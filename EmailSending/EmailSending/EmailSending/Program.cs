using MailKit.Net.Smtp;
using MimeKit;

var message = new MimeMessage();

message.From.Add(new MailboxAddress("Devesh", "devesh.akgec@gmail.com"));
message.To.Add(new MailboxAddress("Recipient", "devesh.omar@gmail.com"));

message.Subject = "Test Email from .NET";

message.Body = new TextPart("plain")
{
    Text = "Hello! This email was sent using Gmail SMTP."
};

using var client = new SmtpClient();

client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);

client.Authenticate(
    "devesh.akgec@gmail.com",
    "tswm acwj mwvs ruyy");

client.Send(message);

client.Disconnect(true);

Console.WriteLine("Email Sent Successfully");