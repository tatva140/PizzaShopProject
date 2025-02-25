namespace Services.Utilities;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Services.Interfaces;


public class EmailService
{
    private readonly IConfiguration _config;
    
     public EmailService(IConfiguration config)
     {
        _config=config;
        
     }
      public async Task SendForgotPasswordEmail(string toEmail,string host, string SenderEmail,string SenderPassword,int SMTPPort,string resetLink)
     {
        var message= new MimeMessage();
        message.From.Add (new MailboxAddress ("PizzaShop", SenderEmail));
        message.To.Add (new MailboxAddress ("Recepient", toEmail));
        message.Subject = "Password Reset Request";

        message.Body = new TextPart ("html") {
        Text = $"<p>Click <a href={resetLink}>here</a> to reset your password</p>"
      };
     using (var smtp = new SmtpClient())
      {
        smtp.Connect(host, SMTPPort, false);

        smtp.Authenticate(SenderEmail,SenderPassword);

        smtp.Send(message);
        smtp.Disconnect(true);
      }

     }

}
