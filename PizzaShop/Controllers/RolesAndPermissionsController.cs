using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services.Service;

namespace PizzaShop.Controllers;

public class RolesAndPermissionsController:Controller
{
    private readonly UserService _userService;
    private readonly RolesAndPermissionsServices _rolesAndPermissionsServices;

    public RolesAndPermissionsController(UserService userService,RolesAndPermissionsServices rolesAndPermissionsServices)
    {
        _userService=userService;
        _rolesAndPermissionsServices=rolesAndPermissionsServices;
    }
   
    public ActionResult Index()
    {
        var roles= _userService.GetRoles().ToList();
        return View(roles);
    }
    public ActionResult Permissions(int id)
    {
        var permissions= _rolesAndPermissionsServices.GetPermissions(id).ToList();
        return View(permissions);
    }

    [HttpPost]
    public ActionResult EditPermissions([FromBody] List<Permission> permissions)
    {
        _rolesAndPermissionsServices.EditPermissions(permissions);
        return RedirectToAction("Permissions","RolesAndPermissions");
    }
}
