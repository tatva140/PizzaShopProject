using System.Security.Claims;
using DAL.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Services.Utilities;

namespace PizzaShop;

public class PermissionsAtrribute : Attribute, IAuthorizationFilter
{
    private readonly string _entity;
    private readonly string _action;

    public PermissionsAtrribute(string entity, string action)
    {
        _entity = entity;
        _action = action;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var permissionService = context.HttpContext.RequestServices.GetService<PermissionService>();
        if (permissionService == null)
        {
            RedirectToNotFound(context);
            return ;  
        }

        var user = context.HttpContext.User;
        var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        if (string.IsNullOrEmpty(role) || !permissionService.HasPermission(role, _entity, _action))
        {
            RedirectToNotFound(context);
            return;

        }
    }

    public void RedirectToNotFound(AuthorizationFilterContext context)
    {
        if(context.HttpContext.Request.Headers["X-Requested-With"]=="XMLHttpRequest"){
            context.Result=new JsonResult(new{
                StatusCode=StatusCodes.Status403Forbidden,
                error="Unauthorized"
            });
        }else{
            context.Result=new ForbidResult();

        }
        

    }

}
