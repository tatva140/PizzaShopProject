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

    public ActionResult AddUser()
    {
        return View();
    }
}

