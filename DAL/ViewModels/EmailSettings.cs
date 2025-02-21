namespace DAL.ViewModels;

public class EmailSettings
{
    public string SMTPServer {get; set;}
    public int SMTPPort {get; set;}
    public string SenderEmail {get; set;}
    public string SenderPassword {get; set;}
    public string host {get; set;}

   
    // "host":"mail.etatvasoft.com",
    // "Encryption" : "STARTTLS"

}
