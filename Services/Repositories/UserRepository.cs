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
         public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u=>u.Email==email);
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
                Country=u.Country,
                State=u.State,
                City=u.City,
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
        
        public void UpdateProfile(string Email,string FirstName,string LastName,string UserName,string Phone,string Country,string State,string City,string Address,string ZipCode,string ProfileImg)
        {
            var user=_context.Users.FirstOrDefault(u=>u.Email==Email);
            user.FirstName=FirstName?? user.FirstName;
            user.LastName=LastName ?? user.LastName;
            user.UserName=UserName ?? user.UserName;
            user.Country=Country ?? user.Country;
            user.State=State ?? user.State;
            user.City=City ?? user.City;
            user.Address=Address ?? user.Address;
            user.ZipCode=ZipCode ?? user.ZipCode;
            user.ProfileImg=ProfileImg ?? user.ProfileImg;
            user.Phone=Phone ?? user.Phone ;

            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }

