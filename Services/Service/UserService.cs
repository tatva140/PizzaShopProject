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
           if(user==null || user.isActive==false){
            return false;
           }
           bool userPassword=BCrypt.Net.BCrypt.Verify(password,user.Password);
           return userPassword;
        }
         public bool ValidateUserByEmail(string email)
        {
            var user = _userRepository.GetByEmail(email);
           if(user==null || user.isActive==false){
            return false;
           }
           return true;
        }
         public UserProfileViewModel GetUserInfo(string email)
        {
            var user = _userRepository.GetByEmail(email);
            return user;
        }
         public (List<UserProfileViewModel>,int totalRecords) GetAllUser(string sortOrder,int pageNumber,int pageSize)
        {
            int totalRecords;
            var users=_userRepository.GetAllUser(sortOrder,pageNumber,pageSize,out totalRecords);
            return  (users,totalRecords);
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
         public void UpdateProfile(UserProfileViewModel userProfileViewModel)
        {
             _userRepository.UpdateProfile(userProfileViewModel);
          
        }
        public List<Country> GetCountries()
        {
            return _userRepository.GetCountries();
        }
        public List<State> GetStateByCountry(int countryId)
        {
            return _userRepository.GetStateByCountry(countryId);
        }
        public List<City> GetCityByState(int stateId)
        {
            return _userRepository.GetCityByState(stateId);
        }
        public List<Role> GetRoles()
        {
            return _userRepository.GetRoles();
        }
        public void AddUser(AddUserViewModel addUserViewModel)
        {
            _userRepository.AddUser(addUserViewModel);
        }
    
}
