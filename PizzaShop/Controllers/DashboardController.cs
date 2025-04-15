namespace PizzaShop.Controllers;
using Services.Utilities;
using Microsoft.AspNetCore.Mvc;
using DAL.ViewModels;
using Services.Service;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using DAL.Models;
using Microsoft.Extensions.FileProviders;


public class DashboardController : Controller
{
    private readonly UserService _userService;
    private readonly EncryptDecrypt _encryptDecrypt;
    private readonly FileUploads _fileUploads;



    public DashboardController(UserService userService, EncryptDecrypt encryptDecrypt, FileUploads fileUploads)
    {
        _userService = userService;
        _encryptDecrypt = encryptDecrypt;
        _fileUploads = fileUploads;
    }

// [Authorize(Roles="Admin,Account Manager")]
    public IActionResult Index(string message)
    {
        if(message!=""){
        ViewBag.error=message;
        }
        return View();

    }

[Authorize]
    [HttpGet]
    public ActionResult UserProfile()
    {
        string email = Request.Cookies["email"];
        UserProfileViewModel user = _userService.GetUserInfo(email);
        return View(user);

    }


    public IActionResult Logout()
    {
        Response.Cookies.Delete("jwtToken");
        Response.Cookies.Delete("email");
        Response.Cookies.Delete("refreshToken");
        return RedirectToAction("Index", "Home");
    }

[Authorize]
    [HttpPost]
    public IActionResult UserProfile(UserProfileViewModel model, IFormFile profileImg)
    {
        if (profileImg != null)
        {
            if (profileImg.Length > 0)
            {
                string fileName = _fileUploads.UploadProfileImage(profileImg);
                model.ProfileImg = "~/images/" + fileName;
            }
        }

        string email = Request.Cookies["email"];
        model.Email = email;
        bool isUpdated = _userService.UpdateProfile(model);
        if (isUpdated)
        {
            TempData["success"] = "Profile Updated Successfully";
        }
        else
        {
            TempData["error"] = "Could not update profile";
        }
        return RedirectToAction("Index", "Dashboard");

    }


    [HttpGet]
    public IActionResult UserInfo(string flag)
    {
        LayoutViewModel layoutViewModel=new LayoutViewModel();
        if(User.Identity.IsAuthenticated==true)
        {

        string email = Request.Cookies["email"];
        UserProfileViewModel user = _userService.GetUserInfo(email);
        layoutViewModel = new LayoutViewModel
        {
            Email = user.Email,
            ProfileImg = user.ProfileImg ?? "~/images/Default_pfp.svg.png",
            Role=user.RoleName,
            Flag=flag
        };
        }

        return PartialView("_UserInfo", layoutViewModel);
    }

    public IActionResult ChangePassword(bool firstTime=false)
    {
        if(firstTime)
        {
            ViewBag.FirstTime=firstTime;
            TempData["error"] = "Please change your password";
            return View();
        }
        if(!User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }
        return View();
    }


    [HttpPost]
    public IActionResult ChangePassword(ResetPasswordViewModel model, bool firstTime)
    {
        string email = Request.Cookies["email"];
        string password = BCrypt.Net.BCrypt.HashPassword(model.newPassword);
        bool isValid = _userService.ChangePassword(password, email, model.currentPassword);
        if (!isValid)
        {
            ViewBag.Message = "Current Password does not match";
            TempData["error"] = "Could not change password";
            return View();
        }
        TempData["success"] = "Changed Password Successfully";
        if(firstTime==true)
        {
        return RedirectToAction("Index", "Home");
        }
        return View();
    }
}
