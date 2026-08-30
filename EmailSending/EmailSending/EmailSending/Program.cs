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



/////////////////////////////////////////////////////////////////////////////////////////
//using MailKit;
//using MailKit.Net.Imap;
//using MailKit.Search;
//using MailKit.Security;

//string email = "devesh.akgec@gmail.com";
//string appPassword = "tswm acwj mwvs ruyy";

//using var client = new ImapClient();

//await client.ConnectAsync(
//    "imap.gmail.com",
//    993,
//    SecureSocketOptions.SslOnConnect);

//await client.AuthenticateAsync(
//    email,
//    appPassword);

//var inbox = client.Inbox;

//await inbox.OpenAsync(FolderAccess.ReadOnly);

//Console.WriteLine($"Total emails: {inbox.Count}");

//int start = Math.Max(0, inbox.Count - 10);

//for (int i = start; i < inbox.Count; i++)
//{
//    var message = await inbox.GetMessageAsync(i);

//    Console.WriteLine("--------------------------------");
//    Console.WriteLine($"Subject : {message.Subject}");
//    Console.WriteLine($"From    : {message.From}");
//    Console.WriteLine($"Date    : {message.Date}");
//    Console.WriteLine($"Body    : {message.TextBody}");
//}

//await client.DisconnectAsync(true);



////-----------------------------------------
///


//using MailKit;
//using MailKit.Net.Imap;
//using MailKit.Search;
//using MailKit.Security;

//string email = "devesh.akgec@gmail.com";
//string appPassword = "tswm acwj mwvs ruyy";

//using var client = new ImapClient();

//await client.ConnectAsync(
//    "imap.gmail.com",
//    993,
//    SecureSocketOptions.SslOnConnect);

//await client.AuthenticateAsync(email, appPassword);

//var inbox = client.Inbox;
//await inbox.OpenAsync(FolderAccess.ReadOnly);

//// Today
//var today = DateTime.Today;

//var uids = await inbox.SearchAsync(
//    SearchQuery.DeliveredAfter(today.AddDays(-1)));

//foreach (var uid in uids)
//{
//    var message = await inbox.GetMessageAsync(uid);

//    // Make sure it is actually today
//    if (message.Date.LocalDateTime.Date == today)
//    {
//        Console.WriteLine("--------------------------------");
//        Console.WriteLine($"Subject: {message.Subject}");
//        Console.WriteLine($"From   : {message.From}");
//        Console.WriteLine($"Date   : {message.Date}");
//        Console.WriteLine($"Body   : {message.TextBody}");
//    }
//}

//await client.DisconnectAsync(true);