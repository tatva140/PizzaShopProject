namespace PizzaShop.Controllers;
using Services.Utilities;
using Microsoft.AspNetCore.Mvc;
using DAL.ViewModels;
using Services.Service;
using Microsoft.AspNetCore.Authorization;


[Authorize]
public class DashboardController :Controller
{
    private readonly UserService _userService;
    private readonly EncryptDecrypt _encryptDecrypt;

    
    public DashboardController(UserService userService,EncryptDecrypt encryptDecrypt)
    {
        _userService=userService;
        _encryptDecrypt=encryptDecrypt;
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
        string roleName= _userService.GetUserRole(email);
        
        var model= new UserProfileViewModel
        {
           Email=user.Email,
           FirstName=user.FirstName,
           LastName=user.LastName,
           UserName=user.UserName,
           Phone=user.Phone,
           Address=user.Address,
           ZipCode=user.ZipCode,
           State=user.State,
           Country=user.Country,
           City=user.City,
           RoleName=roleName

        };
          return View(model);
      
    }
     public IActionResult Logout()
    {
        Response.Cookies.Delete("jwtToken");
        Response.Cookies.Delete("email");
        return RedirectToAction("Index","Home");
    }

    [HttpPost]
    public IActionResult UserProfile(UserProfileViewModel model)
    {
        string email=Request.Cookies["email"];

        _userService.UpdateProfile(email,model.FirstName,model.LastName,model.UserName,model.Phone,model.Country,model.State,model.City,model.Address,model.ZipCode,model.ProfileImg);
        return RedirectToAction("Index","Dashboard");

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
        return View();
       }
        return RedirectToAction("Index","Dashboard");
    }
}
