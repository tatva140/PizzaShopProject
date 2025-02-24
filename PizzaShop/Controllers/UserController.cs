using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Service;
using DAL.ViewModels;
namespace PizzaShop.Controllers;


public class UserController:Controller
{

    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService=userService;
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
    public ActionResult AddUser(AddUserViewModel addUserViewModel)
    {
        _userService.AddUser(addUserViewModel);
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
    public IActionResult EditUsers()
    {
    //   Console.Write(email);
        return View();
    }
}

