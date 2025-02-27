using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
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
        var permissions= _rolesAndPermissionsServices.GetPermissions(id);

        return View(permissions);
    }

    [HttpPost]
    public ActionResult EditPermissions([FromQuery] int roleId,[FromBody] List<Permission> permissions)
    {
        Console.Write(roleId);
        bool isUpdated=_rolesAndPermissionsServices.EditPermissions(permissions);

        if(isUpdated)
        {
            TempData["Message"]="Permissions edited Successfully";
            TempData["MessageType"]="success";
        }
        else
        {
            TempData["Message"]="Could not edit the permissions";
            TempData["MessageType"]="error";
        }
        return Ok(new {redirectUrl=@Url.Action("Permissions","RolesAndPermissions",new{id=roleId})});
    }
}
