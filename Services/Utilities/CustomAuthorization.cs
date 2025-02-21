namespace Services.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using DAL.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;


public class CustomAuthorization : Attribute,IAuthorizationFilter
{
    private readonly string _role;

    public  CustomAuthorization(string role=""){
        _role=role;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var jwtValidate=context.HttpContext.RequestServices.GetService<IJwtService>();
        var token=context.HttpContext.Request.Headers["Authorization"];
         
    //     // var user=context.HttpContext.User;
    //    var abc=jwtValidate.Validate(token,out JwtSecurityToken jwtSecurityToken);
    //    Console.Write(token);

    if(string.IsNullOrEmpty(token))
    {
        context.Result=new UnauthorizedResult();
        return;
    }
    // if(jwtValidate==null){

    //     context.Result=new UnauthorizedResult();
    // }
    //     if(string.IsNullOrEmpty(token) || !jwtValidate.Validate(token,out JwtSecurityToken jwtSecurityToken))
    //     {
    //     Console.WriteLine("Hello");
    //         context.Result=new UnauthorizedResult();
    //         return;
    //     }

        

    //     // foreach (var claim in user.Claims)
    //     // {
    //     //     Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
    //     // }
    //     var roleName=jwtSecurityToken.Claims.FirstOrDefault(c=>c.Type==ClaimTypes.Role)?.Value;
    //             Console.WriteLine(roleName);

    //         if(string.IsNullOrEmpty(roleName))
    //         {
    //             // Console.WriteLine("hi");
    //             context.Result=new UnauthorizedResult();
    //         }
    //     var allowedRoles=_role.Split(',');
    //     if(!allowedRoles.Contains(roleName) )
    //     {
    //             context.Result=new UnauthorizedResult();
    //         }
    }

    

    }
    

