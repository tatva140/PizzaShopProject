using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Services.Repositories;

public class RolesAndPermissions:IRolesAndPermissions
{
    private readonly PizzashopContext _context;

    public RolesAndPermissions(PizzashopContext context)
    {
        _context = context;
    }

    public List<Permission> GetPermissions(int id)
    {
        return _context.Permissions.Where(p=>p.RoleId==id).ToList();
    }
    public void EditPermissions(List<Permission> permissions)
    {
        foreach(var p in permissions)
        {
        var permission=_context.Permissions.FirstOrDefault(c=>c.PermissionId==p.PermissionId);
        if(permission!=null)
        {
            permission.CanAddEdit=p.CanAddEdit;
            permission.CanDelete=p.CanDelete;
            permission.CanView=p.CanView;
        }

        }
        _context.SaveChanges();
    }
}
