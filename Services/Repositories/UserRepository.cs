namespace Services.Repositories;
using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class UserRepository : IUserRepository
{
    private readonly PizzashopContext _context;
    public UserRepository(PizzashopContext context)
    {
        _context = context;
    }
    public void SetRememberMe(string email, bool RememberMe)
    {
        User user = _context.Users.FirstOrDefault(u => u.Email == email && u.IsActive == true);
        user.RememberMe = RememberMe;
    }
    public List<string> GetAllEmail()
    {
        return _context.Users.Where(u => u.IsActive == true).Select(i => i.Email).ToList();
    }
    public UserProfileViewModel GetByEmail(string email)
    {
        var user = (from u in _context.Users
                    join r in _context.Roles on u.RoleId equals r.RoleId
                    join c in _context.Countries on u.Country equals c.CountryId into countryGroup
                    from cg in countryGroup.DefaultIfEmpty()
                    join s in _context.States on u.State equals s.StateId into stateGroup
                    from sg in stateGroup.DefaultIfEmpty()
                    join city in _context.Cities on u.City equals city.CityId into cityGroup
                    from cityg in cityGroup.DefaultIfEmpty()
                    where u.Email == email
                    select new UserProfileViewModel
                    {
                        UserId = u.UserId,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        UserName = u.UserName,
                        Email = u.Email,
                        RoleName = r.RoleName,
                        Address = u.Address,
                        Phone = u.Phone,
                        Country = u.Country ?? 0,
                        CountryName = cg != null ? cg.CountryName : null,
                        State = u.State ?? 0,
                        StateName = sg != null ? sg.StateName : null,
                        City = u.City ?? 0,
                        CityName = cityg != null ? cityg.CityName : null,
                        ZipCode = u.ZipCode,
                        isActive = u.IsActive,
                        ProfileImg = u.ProfileImg,
                        Password = u.Password,
                        isLoggedIn = u.IsLoggedIn
                    }).FirstOrDefault();

        return user;
    }
    public List<UserProfileViewModel> GetAllUser(string search, string sortOrder, int pageNumber, int pageSize, out int totalRecords)
    {
        var query = _context.Users;
        var users = from u in _context.Users
                    join r in _context.Roles on u.RoleId equals r.RoleId
                    where u.IsActive == true
                    select new UserProfileViewModel
                    {
                        UserId = u.UserId,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        UserName = u.UserName,
                        Email = u.Email,
                        RoleName = r.RoleName,
                        Address = u.Address,
                        Phone = u.Phone,
                        Country = u.Country ?? 0,
                        State = u.State ?? 0,
                        City = u.City ?? 0,
                        ZipCode = u.ZipCode,
                        isActive = u.IsActive,
                        ProfileImg = u.ProfileImg,
                        Status = u.Status
                    };
        switch (sortOrder)
        {
            case "desc":
                users = users.OrderByDescending(u => u.FirstName);
                break;
            default:
                users = users.OrderBy(u => u.FirstName);
                break;
        }
        if (search != null)
        {
            List<UserProfileViewModel> searchedUsers = users.Where(u => u.FirstName.ToLower().Contains(search.ToLower())).ToList();
            totalRecords = searchedUsers.Count();
            return searchedUsers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        }
        totalRecords = users.Count();
        return users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }

    public string GetUserRole(string email)
    {
        string? role = (from u in _context.Users
                        join r in _context.Roles on u.RoleId equals r.RoleId
                        where u.Email == email
                        select r.RoleName)
                    .FirstOrDefault();
        return role ?? "";
    }
    public bool DeleteUser(int id)
    {
        User? user = _context.Users.FirstOrDefault(u => u.UserId == id && u.IsActive == true);
        if (user == null) return false;
        user.IsActive = false;
        _context.SaveChanges();
        return true;
    }
    public void ResetPassword(string newPassword, string email)
    {
        User? user = _context.Users.FirstOrDefault(u => u.Email == email);
        if (user == null || user.IsActive == false) return;
        user.Password = newPassword;
        user.IsLoggedIn = true;
        _context.SaveChanges();
    }

    public List<Country> GetCountries()
    {
        return _context.Countries.ToList();
    }
    public List<State> GetStateByCountry(int countryId)
    {
        return _context.States.Where(s => s.CountryId == countryId).ToList();
    }
    public List<City> GetCityByState(int stateId)
    {
        return _context.Cities.Where(c => c.StateId == stateId).ToList();
    }
    public bool UpdateProfile(UserProfileViewModel userProfileViewModel)
    {
        User? user = _context.Users.FirstOrDefault(u => u.Email == userProfileViewModel.Email && u.IsActive == true);
        if (user == null) return false;
        user.FirstName = userProfileViewModel.FirstName ?? user.FirstName;
        user.LastName = userProfileViewModel.LastName ?? user.LastName;
        user.UserName = userProfileViewModel.UserName ?? user.UserName;
        user.Address = userProfileViewModel.Address ?? user.Address;
        user.ZipCode = userProfileViewModel.ZipCode ?? user.ZipCode;
        user.ProfileImg = userProfileViewModel.ProfileImg ?? user.ProfileImg;
        user.Phone = userProfileViewModel.Phone ?? user.Phone;
        if (userProfileViewModel.RoleId != 0)
        {
            user.RoleId = userProfileViewModel.RoleId;
        }
        if (userProfileViewModel.Country != 0)
        {
            user.Country = userProfileViewModel.Country;
        }
        if (userProfileViewModel.State != 0)
        {
            user.State = userProfileViewModel.State;
        }
        if (userProfileViewModel.City != 0)
        {
            user.City = userProfileViewModel.City;
        }
        user.Status = userProfileViewModel.Status;
        user.UpdatedAt = DateTime.Now;

        _context.SaveChanges();
        return true;
    }
    public List<Role> GetRoles(string token)
    {
        if (token != "")
        {
            var handler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;
            string roleName = "";
            if (jwtToken != null)
            {
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
                if (roleClaim != null)
                {
                    roleName = roleClaim;
                }
            }
            if (roleName == "Account Manager")
            {
                return _context.Roles.Where(r => r.RoleName == "Chef" || r.RoleName == "Account Manager").ToList();
            }
        }
        return _context.Roles.ToList();
    }
    public bool AddUser(AddUserViewModel addUserViewModel)
    {
        bool userExists = _context.Users.Any(u => u.Email == addUserViewModel.Email && u.IsActive == true);
        if (userExists) return false;
        User user = new User
        {
            UserId = new Guid().GetHashCode(),
            FirstName = addUserViewModel.FirstName,
            LastName = addUserViewModel.LastName,
            UserName = addUserViewModel.UserName,
            Email = addUserViewModel.Email,
            Phone = addUserViewModel.Phone ?? "",
            City = addUserViewModel.City,
            State = addUserViewModel.State,
            Country = addUserViewModel.Country,
            Address = addUserViewModel.Address ?? "",
            ZipCode = addUserViewModel.ZipCode ?? "",
            ProfileImg = addUserViewModel.ProfileImg ?? "",
            RoleId = addUserViewModel.RoleId,
            Password = addUserViewModel.Password,
            Status = "Active",
            UpdatedAt = DateTime.Now,
            IsLoggedIn = false
        };
        _context.Users.Add(user);
        _context.SaveChanges();
        return true;
    }

}

