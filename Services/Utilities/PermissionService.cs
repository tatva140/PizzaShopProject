using System.Security;
using DAL.Models;
using Services.Repositories;

namespace Services.Utilities;

public class PermissionService
{
    private readonly PizzashopContext _context;

    public PermissionService( PizzashopContext context){
        _context=context;
    }

    public bool HasPermission(string role,string entity, string action){
        if (role == null || role == "" || entity == null || entity == "" || action == null || action == "")
        {
            return false;
        }
        var roleId=_context.Roles.Where(r=>r.RoleName==role).Select(r=>r.RoleId).FirstOrDefault();
        var query = _context.Permissions.Where(p=>p.RoleId==roleId && p.Name==entity).FirstOrDefault();
        bool value;
        if (action == "CanAddEdit")
        {
            value = query.CanAddEdit;
        }
        else if (action == "CanDelete")
        {
            value = query.CanDelete;
        }
        else
        {
            value = query.CanView;

        }

        return value;
    }

}
