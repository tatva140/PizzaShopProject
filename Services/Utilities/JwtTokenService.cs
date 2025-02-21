namespace Services.Utilities;

using Services.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
public class JwtTokenService:IJwtService
{
    private readonly IConfiguration _config;
    public  JwtTokenService(IConfiguration config)
    {
        _config=config;
    }
    public string GenerateToken(string email,string roleName )
    {
           
       
        var jwtSettings= _config.GetSection("Jwt");
        var key=Encoding.UTF8.GetBytes(jwtSettings["Key"]);

        var expiryTime = TimeSpan.FromMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])) ;    

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Email, email),
                        new Claim(ClaimTypes.Role, roleName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    };

    var token = new JwtSecurityToken(
        _config["Jwt:Issuer"],
        _config["Jwt:Audience"],
        claims,
        expires: DateTime.Now.Add(expiryTime),
        signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

     public bool Validate(string jwtToken, out JwtSecurityToken jwtSecurityToken)
        {
            jwtSecurityToken = null;
            if (jwtToken == null)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Convert.ToString(_config["Jwt:Key"]));
            try
            {
                tokenHandler.ValidateToken(jwtToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero

                }, out SecurityToken validatedToken);


                jwtSecurityToken = (JwtSecurityToken)validatedToken;

                if (jwtSecurityToken != null)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
}
