using System.IdentityModel.Tokens.Jwt;

namespace Services.Interfaces;

public interface IJwtService
{
     bool Validate(string jwtToken, out JwtSecurityToken jwtSecurityToken);
     string GenerateToken(string email,string roleName );


}
