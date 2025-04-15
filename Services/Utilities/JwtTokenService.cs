namespace Services.Utilities;

using Services.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using DAL.Models;
using DAL.ViewModels;
using Services.Service;

public class JwtTokenService : IJwtService
{
    private readonly IConfiguration _config;
    private readonly PizzashopContext _context;
    private readonly UserService _userService;

    public JwtTokenService(IConfiguration config, PizzashopContext context, UserService userService)
    {
        _config = config;
        _context = context;
        _userService = userService;
    }
    public string GenerateToken(string email)
    {

        string roleName = _userService.GetUserRole(email);

        var jwtSettings = _config.GetSection("Jwt");
        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

        var expiryTime = TimeSpan.FromMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"]));

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
    public string GenerateRefreshToken()
    {
        string refresh_token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        return refresh_token;
    }
    public TokenResponse RefreshToken(string refreshToken)
    {
        User user = _context.Users.FirstOrDefault(u => u.RefreshToken == refreshToken && u.IsActive == true);
        // string newRefreshToken = GenerateRefreshToken();
        // user.RefreshToken = newRefreshToken;
        // DateTime refreshTokenExpiryTime = user.RememberMe ?? false ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(1);
        var newjwtToken = GenerateToken(user.Email);
        
        // user.ExpiryTime = refreshTokenExpiryTime;
        _context.SaveChanges();
        return new TokenResponse
        {
            jwtToken = newjwtToken,
            // expiryTime = refreshTokenExpiryTime,
            // refreshToken = newRefreshToken,
        };

    }
    public void SaveRefreshToken(string refresh_token, string email, DateTime refreshTokenExpiryTime)
    {

        User user = _context.Users.FirstOrDefault(u => u.Email == email && u.IsActive == true);
        user.RefreshToken = refresh_token;
        user.ExpiryTime = refreshTokenExpiryTime;
        _context.SaveChanges();
    }

    public string GetRoleFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        return roleClaim?.Value;
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
