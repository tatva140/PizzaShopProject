using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services.Service;

namespace PizzaShop.Controllers;

public class RolesAndPermissionsController:Controller
{
    private readonly UserService _userService;

    public RolesAndPermissionsController(UserService userService)
    {
        _userService=userService;
    }
    public ActionResult Index()
    {
        var roles= _userService.GetRoles().ToList();
        return View(roles);
    }
    public ActionResult Permissions()
    {
        
        return View();
    }
}
