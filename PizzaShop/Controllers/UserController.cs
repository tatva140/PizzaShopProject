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

    public UserController(UserService userService,EmailService emailService,EncryptDecrypt encryptDecrypt,EmailSettings emailSettings)
    {
        _userService=userService;
        _emailService=emailService;
        _encryptDecrypt=encryptDecrypt;
        _emailSettings=emailSettings;
    }

[HttpGet]

      public ActionResult Index(string sortOrder,int pageNumber=1,int pageSize=2)
    {
        
        ViewBag.sort = String.IsNullOrEmpty(sortOrder) ? "desc" : "";
        var (users,totalRecords)=_userService.GetAllUser(sortOrder,pageNumber,pageSize);
        ViewBag.PageNumber=pageNumber;
        ViewBag.PageSize=pageSize;
        ViewBag.TotalPages=(int)Math.Ceiling((double)totalRecords/pageSize);
        ViewBag.AvailableSize=new List<int>{2,4,15,20};
        return View(users);
    }

[HttpPost]
      public IActionResult DeleteUser([FromQuery] int id)
    {
        bool deleted=_userService.DeleteUser(id);
        return RedirectToAction("Index","User");
    }

[HttpGet]
    public ActionResult AddUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult>  AddUser(AddUserViewModel addUserViewModel)
    {
        _userService.AddUser(addUserViewModel);
        string email=_encryptDecrypt.Encrypt(addUserViewModel.Email);
        string resetLink= Url.Action("ResetPassword","Home",new  {email=email}, Request.Scheme);
        await _emailService.SendForgotPasswordEmail(addUserViewModel.Email,_emailSettings.host,_emailSettings.SenderEmail,_emailSettings.SenderPassword,_emailSettings.SMTPPort,resetLink);
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
    public IActionResult EditUsers([FromQuery] string email)
    {
        //   if(!ModelState.IsValid)
        // {
        // var errors = ModelState.Values.SelectMany(v => v.Errors);
        // foreach (var error in errors)
        // {
        //     Console.WriteLine(error.ErrorMessage);
        // }
        // return RedirectToAction("UserProfile","Dashboard");
        // }
        var user = _userService.GetUserInfo(email);
        Console.Write(user.StateName);
        return View(user);   
    }

  [HttpPost]
    public IActionResult EditUsers(UserProfileViewModel model)
    {
        _userService.UpdateProfile(model);
        return RedirectToAction("Index","Dashboard");
    }  
}

