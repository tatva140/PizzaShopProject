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
      public ActionResult Index()
    {
        var users=_userService.GetAllUser();
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

