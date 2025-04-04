using DAL.Models;

namespace DAL.ViewModels;

public class PermissionsViewModel
{
    public List<Permission> permission {get;set;}

    public string RoleName {get;set;}

    public int RoleId {get;set;}

    public string CurrUserRole {get;set;}

}
