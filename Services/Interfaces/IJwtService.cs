using System.IdentityModel.Tokens.Jwt;
using DAL.ViewModels;

namespace Services.Interfaces;

public interface IJwtService
{
     bool Validate(string jwtToken, out JwtSecurityToken jwtSecurityToken);
     string GenerateToken(string email);

     TokenResponse RefreshToken(string refreshToken);
     string GenerateRefreshToken();
     void SaveRefreshToken(string refresh_token, string email, DateTime refreshTokenExpiryTime);
}
