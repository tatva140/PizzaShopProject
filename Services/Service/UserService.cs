namespace Services.Service;
using Services.Utilities;
using Services.Interfaces;
using System.Threading.Tasks;
using DAL.Models;
using DAL.ViewModels;

public class UserService
{
  
        private readonly IUserRepository _userRepository;
        private readonly EncryptDecrypt _encryptDecrypt;

        public UserService( IUserRepository userRepository,EncryptDecrypt encryptDecrypt)
        {
          _userRepository=userRepository;
          _encryptDecrypt=encryptDecrypt;
        }
         public bool ValidateUser(string email, string password)
        {

            var user = _userRepository.GetByEmail(email);
           if(user==null){
            return false;
           }
           bool userPassword=BCrypt.Net.BCrypt.Verify(password,user.Password);
           return userPassword;
        }
         public bool ValidateUserByEmail(string email)
        {
            var user = _userRepository.GetByEmail(email);
           if(user==null){
            return false;
           }
           return true;
        }
         public User GetUserInfo(string email)
        {
            var user = _userRepository.GetByEmail(email);
            return user;
        }
         public List<UserProfileViewModel> GetAllUser()
        {
            return  _userRepository.GetAllUser();
        }
         public string GetUserRole(string email)
        {
           return _userRepository.GetUserRole(email);
        }
         public bool DeleteUser(int id)
        {
           return _userRepository.DeleteUser(id);
        }
         public void ResetPassword(string newPassword,string email)
        {
            _userRepository.ResetPassword(newPassword,email); 
        }
         public bool ChangePassword(string newPassword,string email,string currentPassword)
        {
            var user = _userRepository.GetByEmail(email);
            
           bool userPassword=BCrypt.Net.BCrypt.Verify(currentPassword,user.Password);
           
           if(userPassword || user.Password==currentPassword)
           {
            _userRepository.ResetPassword(newPassword,email);
            return true;
           }

            return false;
          
        }
         public void UpdateProfile(string Email,string FirstName,string LastName,string UserName,string Phone,string Country,string State,string City,string Address,string ZipCode,string ProfileImg)
        {
             _userRepository.UpdateProfile(Email,FirstName,LastName,UserName,Phone,Country,State,City,Address,ZipCode,ProfileImg);
          
        }
    
}
