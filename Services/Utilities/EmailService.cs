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
      public async Task SendForgotPasswordEmail(string toEmail,string host, string SenderEmail,string SenderPassword,int SMTPPort,string resetLink="",string username="",string password="")
     {
        var message= new MimeMessage();
        message.From.Add (new MailboxAddress ("PizzaShop", SenderEmail));
        message.To.Add (new MailboxAddress ("Recepient", toEmail));
        message.Subject = "Password Reset Request";

        if(resetLink!=null)
        {
          message.Body = new TextPart ("html") {
          Text = $"<p>Click <a href={resetLink}>here</a> to reset your password</p>"
          };
        }else{
          message.Body = new TextPart ("html") {
          Text = $"<h5>Welcome to Pizza Shop</h5> <h5>Please find the details below for login into your account</h5><div style='border:2px solid black;' class='p-3'> <h4>Login Details:<h4> <h5>Username:{username}</h5> <h5>Password:{password}</h5></div><h4>If you encounter any issues or have any question,please do not hesitate to contact our support team</h4>"
          };
        }
       
     using (var smtp = new SmtpClient())
      {
        smtp.Connect(host, SMTPPort, false);

        smtp.Authenticate(SenderEmail,SenderPassword);

        smtp.Send(message);
        smtp.Disconnect(true);
      }

     }

}
