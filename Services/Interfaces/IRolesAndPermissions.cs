using DAL.Models;

namespace Services.Interfaces;

public interface IRolesAndPermissions
{
    List<Permission> GetPermissions(int id);
  
  void EditPermissions(List<Permission> permissions);
}
