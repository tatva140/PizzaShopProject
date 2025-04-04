using System.Security.Claims;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Services.Repositories;

public class RolesAndPermissions : IRolesAndPermissions
{
    private readonly PizzashopContext _context;

    public RolesAndPermissions(PizzashopContext context)
    {
        _context = context;
    }

    public PermissionsViewModel GetPermissions(int id,string token)
    {
        var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;
            string currRoleName = "";
            if (jwtToken != null)
            {
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (roleClaim != null)
                {
                    currRoleName = roleClaim;
                }
            }
        string? roleName = (from r in _context.Roles
                           where r.RoleId == id
                           select r.RoleName
                        ).FirstOrDefault();

        List<Permission> permissions = (from p in _context.Permissions
                           where p.RoleId == id
                           select new Permission
                           {
                               PermissionId = p.PermissionId,
                               CanAddEdit = p.CanAddEdit,
                               CanDelete = p.CanDelete,
                               CanView = p.CanView,
                               Name = p.Name,
                           }).ToList();
        return new PermissionsViewModel
        {
            RoleName = roleName,
            permission = permissions,
            RoleId = id,
            CurrUserRole= currRoleName
        };
    }
    public bool EditPermissions(List<Permission> permissions)
    {
        foreach (var p in permissions)
        {
            Permission permission = _context.Permissions.FirstOrDefault(c => c.PermissionId == p.PermissionId);
            if (permission == null) return false;
            if (permission != null)
            {
                permission.CanAddEdit = p.CanAddEdit;
                permission.CanDelete = p.CanDelete;
                permission.CanView = p.CanView;
            }
        }
        _context.SaveChanges();
        return true;
    }
}
