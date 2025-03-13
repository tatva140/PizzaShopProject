namespace DAL.ViewModels;

public class TokenResponse
{
    public string jwtToken{get;set;}
    public string refreshToken{get;set;}
    public DateTime expiryTime{get;set;}
}
