namespace Services.Repositories;
using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;


public class UserRepository: IUserRepository
    {
        private readonly PizzashopContext _context;
        public UserRepository(PizzashopContext context)
        {
            _context=context;
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
                    CountryName = cg != null ? cg.CountryName : null, // Get country name if present
                    State = u.State ?? 0,
                    StateName = sg != null ? sg.StateName : null, // Get state name if present
                    City = u.City ?? 0,
                    CityName = cityg != null ? cityg.CityName : null, // Get city name if present
                    ZipCode = u.ZipCode,
                    isActive = u.IsActive,
                    ProfileImg = u.ProfileImg,
                    Password=u.Password
                }).FirstOrDefault();

    return user;
}
         public List<UserProfileViewModel> GetAllUser(string sortOrder,int pageNumber,int pageSize,out int totalRecords)
        {
            var query=_context.Users;
            totalRecords=query.Count();
            var users=from u in _context.Users
            join r in _context.Roles on u.RoleId equals r.RoleId
            select new UserProfileViewModel
            {
                UserId= u.UserId,
                FirstName=u.FirstName,
                LastName=u.LastName,
                UserName=u.UserName,
                Email=u.Email,
                RoleName=r.RoleName,
                Address=u.Address,
                Phone=u.Phone,
                Country=u.Country ?? 0,
                State=u.State??0,
                City=u.City??0,
                ZipCode=u.ZipCode,
                isActive=u.IsActive,
                ProfileImg=u.ProfileImg

            };
            switch(sortOrder)
            {
                case "desc":
                users=users.OrderByDescending(u=>u.FirstName);
                break;
                default:
                users=users.OrderBy(u=>u.FirstName);
                break;
            }
            return users.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();
        }
         public string GetUserRole(string email)
        {
            var role=  ( from u in _context.Users
                        join r in _context.Roles on u.RoleId equals r.RoleId
                        where u.Email==email
                        select r.RoleName)
                        .FirstOrDefault();
                        Console.Write(role);
            return role;
        }
          public bool DeleteUser(int id)
        {
            var user=_context.Users.Find(id);
            if(user==null)return false;
            user.IsActive=false;
            _context.Users.Update(user);
            _context.SaveChanges();
            return true;
        }
         public void ResetPassword(string newPassword,string email)
        {
            var user= _context.Users.FirstOrDefault(u=>u.Email==email);  
            if(user==null|| user.IsActive==false)return;
           user.Password= newPassword;
           _context.SaveChanges();
        }
        
        public List<Country> GetCountries()
        {
            return _context.Countries.ToList();
        }
        public List<State> GetStateByCountry(int countryId)
        {
            return _context.States.Where(s=>s.CountryId==countryId).ToList();
        }
        public List<City> GetCityByState(int stateId)
        {
            return _context.Cities.Where(c=>c.StateId==stateId).ToList();
        }
        public void UpdateProfile(UserProfileViewModel userProfileViewModel)
        {
            var user=_context.Users.FirstOrDefault(u=>u.Email==userProfileViewModel.Email);
            user.FirstName=userProfileViewModel.FirstName?? user.FirstName;
            user.LastName=userProfileViewModel.LastName ?? user.LastName;
            user.UserName=userProfileViewModel.UserName ?? user.UserName;
            user.Country=userProfileViewModel.Country ;
            user.State=userProfileViewModel.State;
            user.City=userProfileViewModel.City;
            user.Address=userProfileViewModel.Address ?? user.Address;
            user.ZipCode=userProfileViewModel.ZipCode ?? user.ZipCode;
            user.ProfileImg=userProfileViewModel.ProfileImg ?? user.ProfileImg;
            user.Phone=userProfileViewModel.Phone ?? user.Phone ;

            _context.Users.Update(user);
            _context.SaveChanges();
        }
        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }
        public void AddUser(AddUserViewModel addUserViewModel)
        {
         
            var user=new User{
                UserId=new Guid().GetHashCode(),
                FirstName=addUserViewModel.FirstName,
                LastName=addUserViewModel.LastName,
                UserName=addUserViewModel.UserName,
                Email=addUserViewModel.Email,
                Phone=addUserViewModel.Phone??"",
                City=addUserViewModel.City,
                State=addUserViewModel.State,
                Country=addUserViewModel.Country,
                Address=addUserViewModel.Address?? "",
                ZipCode=addUserViewModel.ZipCode?? "",
                ProfileImg=addUserViewModel.ProfileImg?? "",
                RoleId=addUserViewModel.RoleId
            };
             _context.Users.Add(user);
            _context.SaveChanges();
        }
    }

