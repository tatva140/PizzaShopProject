using DAL.Models;
using DAL.ViewModels;

namespace Services.Interfaces;

public interface IRolesAndPermissions
{
  PermissionsViewModel GetPermissions(int id);
  
  bool EditPermissions(List<Permission> permissions);
}
