using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DAL.ViewModels;
using Services.Service;
using Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using Services.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using DAL.Models;
using System.Threading.Tasks;
using System.Net.Http.Headers;
namespace PizzaShop.Controllers;


public class HomeController : Controller
{
    private readonly UserService _userService;
    private readonly EmailSettings _emailSettings;
    private readonly EmailService _emailService;
    private readonly EncryptDecrypt _encryptDecrypt;
    private readonly JwtTokenService _jwtTokenService;



    public HomeController(UserService userService, EmailSettings emailSettings, EmailService emailService, JwtTokenService jwtTokenService, EncryptDecrypt encryptDecrypt)
    {
        _userService = userService;
        _emailSettings = emailSettings;
        _emailService = emailService;
        _jwtTokenService = jwtTokenService;
        _encryptDecrypt = encryptDecrypt;

    }


    public IActionResult Index()
    {
        if (Request.Cookies.ContainsKey("refreshToken"))
        {
            return RedirectToAction("Index", "Dashboard");
        }
        return View();
    }

    [HttpPost]
    public IActionResult Index(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            bool isValidUser = _userService.ValidateUser(model.Email, model.Password);
            _userService.SetRememberMe(model.Email, model.RememberMe);
            if (isValidUser)
            {

                DateTime refreshTokenExpiryTime = model.RememberMe ? DateTime.UtcNow.AddDays(30) : DateTime.UtcNow.AddHours(1);

                string token = _jwtTokenService.GenerateToken(model.Email);
                string refresh_token = _jwtTokenService.GenerateRefreshToken();
                _jwtTokenService.SaveRefreshToken(refresh_token, model.Email, refreshTokenExpiryTime);

                Response.Cookies.Append("jwtToken", token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = refreshTokenExpiryTime,
                });

                Response.Cookies.Append("email", model.Email, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = refreshTokenExpiryTime
                });
                Response.Cookies.Append("refreshToken", refresh_token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = refreshTokenExpiryTime
                });
                return RedirectToAction("Index", "Dashboard");
            }
            ViewBag.Message = "Invalid credentials";
            return View(model);
        }
        return RedirectToAction("Index");
    }
    public IActionResult RefreshToken([FromBody] string refresh_token)
    {
        var content = _jwtTokenService.RefreshToken(refresh_token);
        return Ok(new { jwtToken = content.jwtToken, refreshToken = content.refreshToken, expiryTime = content.expiryTime });
    }
    [HttpGet]
    public IActionResult ForgotPassword(string? email)
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        bool isValidUser = _userService.ValidateUserByEmail(model.Email);
        if (!isValidUser)
        {
            ViewBag.Message = "Email not found";
            return View();
        }
        string email = _encryptDecrypt.Encrypt(model.Email);
        string resetLink = Url.Action("ResetPassword", "Home", new { email = email }, Request.Scheme);
        await _emailService.SendForgotPasswordEmail(model.Email, _emailSettings.host, _emailSettings.SenderEmail, _emailSettings.SenderPassword, _emailSettings.SMTPPort, resetLink);
        ViewBag.Message = "A reset password link has been sent on this email";
        return View(model);
    }

    [HttpGet]

    public IActionResult ResetPassword([FromQuery] string email)
    {
        string emailDecrypt = _encryptDecrypt.Decrypt(email);
        var model = new ResetPasswordViewModel
        {
            Email = emailDecrypt
        };

        return View(model);
    }

    [HttpPost]
    // [Route("ResetPassword")]
    public IActionResult ResetPassword(ResetPasswordViewModel model)
    {
        string password = BCrypt.Net.BCrypt.HashPassword(model.newPassword);

        _userService.ResetPassword(password, model.Email);
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult PageNotFound()
    {
        return PartialView("_PageNotFound");
    }


    // [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    // public IActionResult Error()
    // {
    //     return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    // }
}
