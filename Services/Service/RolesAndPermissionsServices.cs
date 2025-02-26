using DAL.Models;
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

    public List<Permission> GetPermissions([FromQuery] int id)
    {
        return _rolesAndPermissions.GetPermissions(id);
    }

    public void EditPermissions(List<Permission> permissions)
    {
        _rolesAndPermissions.EditPermissions(permissions);
    }
}
