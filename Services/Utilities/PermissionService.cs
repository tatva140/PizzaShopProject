using System.Security;
using DAL.Models;
using Services.Repositories;

namespace Services.Utilities;

public class PermissionService
{
    private readonly PizzashopContext _context;

    // private readonly PermissionRepository _permissionRepository;
    public PermissionService( PizzashopContext context){
        _context=context;
    }

    public bool HasPermission(string role,string entity, string action){
       if (role == null || role == "" || entity == null || entity == "" || action == null || action == "")
        {
            return false;
        }
        var query = (from p in _context.Permissions
                     join r in _context.Roles on role equals r.RoleName
                     where r.RoleName == role && p.Name == entity
                     select p);
        bool value;
        if (action == "CanAddEdit")
        {
            value = query.Select(p => p.CanAddEdit).FirstOrDefault();
        }
        else if (action == "CanDelete")
        {
            value = query.Select(p => p.CanDelete).FirstOrDefault();
        }
        else
        {
            value = query.Select(p => p.CanView).FirstOrDefault();

        }

        return value;
    }

}
