namespace PizzaShop.Controllers;
using Services.Utilities;
using Microsoft.AspNetCore.Mvc;
using DAL.ViewModels;
using Services.Service;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using DAL.Models;
using Microsoft.Extensions.FileProviders;

[Authorize]
public class DashboardController :Controller
{
    private readonly UserService _userService;
    private readonly EncryptDecrypt _encryptDecrypt;
    private readonly FileUploads _fileUploads;


    
    public DashboardController(UserService userService,EncryptDecrypt encryptDecrypt,FileUploads fileUploads)
    {
        _userService=userService;
        _encryptDecrypt=encryptDecrypt;
        _fileUploads=fileUploads;
    }

    public IActionResult Index()
    {
        //  var user = _userService.ValidateUser(model.Email);
          return View();
      
    }

[HttpGet]
    public ActionResult UserProfile()
    {
        string email=Request.Cookies["email"];
        var user = _userService.GetUserInfo(email);
        return View(user);
      
    }
     public IActionResult Logout()
    {
        Response.Cookies.Delete("jwtToken");
        Response.Cookies.Delete("email");
        return RedirectToAction("Index","Home");
    }

    [HttpPost]
    public IActionResult UserProfile(UserProfileViewModel model,IFormFile profileImg)
    {
        Console.Write(model.isActive);
          if (profileImg != null)
            {
             if (profileImg.Length > 0)
            {
                string fileName=_fileUploads.UploadProfileImage(profileImg);
                model.ProfileImg="/images/"+fileName;
            }
     }
       
        string email=Request.Cookies["email"];
        model.Email=email;
        bool isUpdated=_userService.UpdateProfile(model);
        if(isUpdated)
        {
            TempData["Message"]="Profile Updated Successfully";
            TempData["MessageType"]="success";
        }else{
            TempData["Message"]="Could not update profile";
            TempData["MessageType"]="error";
        }
        return RedirectToAction("Index","Dashboard");

    }

    [HttpGet]
      public IActionResult UserInfo()
        {
            string email=Request.Cookies["email"];
            var user=_userService.GetUserInfo(email);
            LayoutViewModel layoutViewModel =new LayoutViewModel
            {
                Email=user.Email,
                ProfileImg=user.ProfileImg
            };
       
            return PartialView("_UserInfo",layoutViewModel);
        }
     public IActionResult ChangePassword()
    {
        return View();
    }

    [HttpPost]
     public IActionResult ChangePassword(ResetPasswordViewModel model)
    {
        string email=Request.Cookies["email"];
        string password=BCrypt.Net.BCrypt.HashPassword(model.newPassword);
       bool isValid= _userService.ChangePassword(password,email,model.currentPassword);
       if(!isValid)
       {
        ViewBag.Message="Current Password does not match";
        TempData["Message"]="Could not change password";
        TempData["MessageType"]="error";
        return View();
       }
        TempData["Message"]="Changed Password Successfully";
        TempData["MessageType"]="success";
        return View();
    }
}
