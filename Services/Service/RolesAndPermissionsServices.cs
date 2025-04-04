using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Services.Service;

public class RolesAndPermissionsServices
{
    private readonly IRolesAndPermissions _rolesAndPermissions;

    public RolesAndPermissionsServices(IRolesAndPermissions rolesAndPermissions)
    {
        _rolesAndPermissions = rolesAndPermissions;
    }

    public PermissionsViewModel GetPermissions([FromQuery] int id,string token)
    {
        return _rolesAndPermissions.GetPermissions(id,token);
    }

    public bool EditPermissions(List<Permission> permissions)
    {
        return _rolesAndPermissions.EditPermissions(permissions);
    }
}
