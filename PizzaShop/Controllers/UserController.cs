using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Service;
using Services.Utilities;
using DAL.ViewModels;
namespace PizzaShop.Controllers;


public class UserController:Controller
{

    private readonly UserService _userService;
    private readonly EmailService _emailService;
    private readonly EncryptDecrypt _encryptDecrypt;
    private readonly EmailSettings _emailSettings;
    private readonly FileUploads _fileUploads;


    public UserController(UserService userService,EmailService emailService,EncryptDecrypt encryptDecrypt,EmailSettings emailSettings,FileUploads fileUploads)
    {
        _userService=userService;
        _emailService=emailService;
        _encryptDecrypt=encryptDecrypt;
        _emailSettings=emailSettings;
        _fileUploads=fileUploads;
    }

[HttpGet]

      public ActionResult Index(string search,string sortOrder,int pageNumber=1,int pageSize=2)
    {
        Console.Write(search);
        ViewBag.sort = String.IsNullOrEmpty(sortOrder) ? "desc" : "";
        var (users,totalRecords)=_userService.GetAllUser(sortOrder,pageNumber,pageSize);
        ViewBag.PageNumber=pageNumber;
        ViewBag.PageSize=pageSize;
        ViewBag.TotalPages=(int)Math.Ceiling((double)totalRecords/pageSize);
        ViewBag.AvailableSize=new List<int>{2,4,15,20};
       
        return View(users);
    }

[HttpPost]
      public IActionResult DeleteUser([FromQuery] int id,[FromQuery] int pageNumber,[FromQuery] int pageSize)
    {
        bool deleted=_userService.DeleteUser(id);
        if(deleted)
        {
            TempData["Message"]="User Deleted Successfully";
            TempData["MessageType"]="success";
        }
        else
        {
            TempData["Message"]="User does not exist";
            TempData["MessageType"]="error";
        }
        return RedirectToAction("Index","User",new{pageNumber=pageNumber, pageSize=pageSize});
    }

[HttpGet]
    public ActionResult AddUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult>  AddUser(AddUserViewModel addUserViewModel,IFormFile profileImg)
    {
        if (profileImg != null)
        {
            if (profileImg.Length > 0)
            {
                string fileName=_fileUploads.UploadProfileImage(profileImg);
                addUserViewModel.ProfileImg="/images/"+fileName;

            }
        }
        bool userValid=_userService.AddUser(addUserViewModel);
        if(userValid)
        {
            TempData["Message"]="User Added Successfully";
            TempData["MessageType"]="success";
        }
        else
        {
            TempData["Message"]="User Already Exists";
            TempData["MessageType"]="error";
        }
        string email=_encryptDecrypt.Encrypt(addUserViewModel.Email);
        string resetLink= Url.Action("ResetPassword","Home",new  {email=email}, Request.Scheme);
        await _emailService.SendForgotPasswordEmail(addUserViewModel.Email,_emailSettings.host,_emailSettings.SenderEmail,_emailSettings.SenderPassword,_emailSettings.SMTPPort,resetLink,addUserViewModel.Email,addUserViewModel.Password);
        return RedirectToAction("Index","User");
    }
    public ActionResult GetCountries()
    {
        var countries=_userService.GetCountries();
        return Json(countries);
    }
    public ActionResult GetStateByCountry([FromQuery]int countryId)
    {
        var states=_userService.GetStateByCountry(countryId);
        return Json(states);
    }
    public ActionResult GetCityByState(int stateId)
    {
        var cities=_userService.GetCityByState(stateId);
        return Json(cities);
    }
    public ActionResult GetRoles()
    {
       var roles= _userService.GetRoles();
       return Json(roles);
    }

[HttpGet]
    public IActionResult EditUsers([FromQuery] string email,[FromQuery] int pageNumber,[FromQuery] int pageSize)
    {
        var user = _userService.GetUserInfo(email);
        ViewBag.PageNumber=pageNumber;
        ViewBag.PageSize=pageSize;
        return View(user);   
    }

  [HttpPost]
    public IActionResult EditUsers([FromQuery] int pageNumber,[FromQuery] int pageSize,UserProfileViewModel model,IFormFile profileImg)
    {
        Console.Write(pageNumber);
        Console.Write(pageSize);
         if (profileImg != null)
        {
            if (profileImg.Length > 0)
            {
                string fileName=_fileUploads.UploadProfileImage(profileImg);
                model.ProfileImg="/images/"+fileName;
            }
        }
        bool isUpdated=_userService.UpdateProfile(model);
        if(isUpdated)
        {
            TempData["Message"]="User edited Successfully";
            TempData["MessageType"]="success";
        }else
        {
            TempData["Message"]="Could not edit user";
            TempData["MessageType"]="error";
        }
        return RedirectToAction("Index","User",new{pageNumber=pageNumber, pageSize=pageSize});
    }  
}

