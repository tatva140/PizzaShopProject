using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Service;
using Services.Utilities;
using DAL.ViewModels;
namespace PizzaShop.Controllers;


public class UserController : Controller
{

    private readonly UserService _userService;
    private readonly EmailService _emailService;
    private readonly EncryptDecrypt _encryptDecrypt;
    private readonly EmailSettings _emailSettings;
    private readonly FileUploads _fileUploads;


    public UserController(UserService userService, EmailService emailService, EncryptDecrypt encryptDecrypt, EmailSettings emailSettings, FileUploads fileUploads)
    {
        _userService = userService;
        _emailService = emailService;
        _encryptDecrypt = encryptDecrypt;
        _emailSettings = emailSettings;
        _fileUploads = fileUploads;
    }
    [PermissionsAtrribute("User", "CanAddEdit")]
    public ActionResult AddEdit()
    {
        return Ok();
    }
    [PermissionsAtrribute("User", "CanDelete")]
    public ActionResult Delete()
    {
        return Ok();
    }
    [PermissionsAtrribute("User", "CanView")]
    public ActionResult ViewPermission()
    {
        return Ok();
    }
    [HttpGet]
    public ActionResult Index(string search, string sortOrder, int pageNumber = 1, int pageSize = 2)
    {
        ViewBag.sort = String.IsNullOrEmpty(sortOrder) ? "desc" : "";
        var (users, totalRecords) = _userService.GetAllUser(search, sortOrder, pageNumber, pageSize);
        ViewBag.PageNumber = pageNumber;
        ViewBag.PageSize = pageSize;
        ViewBag.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
        ViewBag.AvailableSize = new List<int> { 2, 4, 15, 20 };
        ViewBag.Search = search;
        return View(users);
    }
    [HttpGet]

    public JsonResult CheckUserExists(string email)
    {
        List<string> existingEmail = _userService.GetAllEmail();
        bool IsAvailable = !existingEmail.Contains(email.ToLower());
        return Json(IsAvailable);
    }
    [HttpPost]
    public IActionResult DeleteUser([FromQuery] int id, [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        bool deleted = _userService.DeleteUser(id);
        if (deleted)
        {
            TempData["success"] = "User Deleted Successfully";
        }
        else
        {
            TempData["error"] = "User does not exist";
        }
        return RedirectToAction("Index", "User", new { pageNumber = pageNumber, pageSize = pageSize });
    }

    [HttpGet]
    public ActionResult AddUser()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddUser(AddUserViewModel addUserViewModel, IFormFile profileImg)
    {
        if (profileImg != null)
        {
            if (profileImg.Length > 0)
            {
                string fileName = _fileUploads.UploadProfileImage(profileImg);
                addUserViewModel.ProfileImg = "~/images/" + fileName;

            }
        }
        string password = addUserViewModel.Password;
        bool userValid = _userService.AddUser(addUserViewModel);
        if (userValid)
        {
            TempData["success"] = "User Added Successfully";
        }
        else
        {
            TempData["error"] = "User Already Exists";
        }
        await _emailService.SendForgotPasswordEmail(addUserViewModel.Email, _emailSettings.host, _emailSettings.SenderEmail, _emailSettings.SenderPassword, _emailSettings.SMTPPort, null, addUserViewModel.Email, password);
        return RedirectToAction("Index", "User");
    }
    public ActionResult GetCountries()
    {
        List<Country> countries = _userService.GetCountries();
        return Json(countries);
    }
    public ActionResult GetStateByCountry([FromQuery] int countryId)
    {
        List<State> states = _userService.GetStateByCountry(countryId);
        return Json(states);
    }
    public ActionResult GetCityByState(int stateId)
    {
        List<City> cities = _userService.GetCityByState(stateId);
        return Json(cities);
    }
    public ActionResult GetRoles()
    {
        List<Role> roles = _userService.GetRoles();
        return Json(roles);
    }

    [HttpGet]
    public IActionResult EditUsers([FromQuery] string email, [FromQuery] int pageNumber, [FromQuery] int pageSize)
    {
        UserProfileViewModel user = _userService.GetUserInfo(email);
        ViewBag.PageNumber = pageNumber;
        ViewBag.PageSize = pageSize;
        return View(user);
    }

    [HttpPost]
    public IActionResult EditUsers([FromQuery] int pageNumber, [FromQuery] int pageSize, UserProfileViewModel model, IFormFile profileImg)
    {
        if (profileImg != null)
        {
            if (profileImg.Length > 0)
            {
                string fileName = _fileUploads.UploadProfileImage(profileImg);
                model.ProfileImg = "~/images/" + fileName;
            }
        }
        bool isUpdated = _userService.UpdateProfile(model);
        if (isUpdated)
        {
            TempData["success"] = "User edited Successfully";
        }
        else
        {
            TempData["error"] = "Could not edit user";
        }
        return RedirectToAction("Index", "User", new { pageNumber = pageNumber, pageSize = pageSize });
    }
}

