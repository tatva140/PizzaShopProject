namespace Services.Interfaces;
using DAL.Models;
using DAL.ViewModels;

public interface IUserRepository
{
    User GetByEmail(string email);
    void ResetPassword(string newPassword, string email);
    string GetUserRole( string email);
    
    List<UserProfileViewModel> GetAllUser();
    bool DeleteUser(int id);
    void UpdateProfile(string Email,string FirstName,string LastName,string UserName,string Phone,string Country,string State,string City,string Address,string ZipCode,string ProfileImg);


}

