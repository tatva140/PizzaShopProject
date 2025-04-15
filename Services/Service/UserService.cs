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

    public UserService(IUserRepository userRepository, EncryptDecrypt encryptDecrypt)
    {
        _userRepository = userRepository;
        _encryptDecrypt = encryptDecrypt;
    }
    public void SetRememberMe(string email, bool RememberMe)
    {
        _userRepository.SetRememberMe(email, RememberMe);
    }
    public CustomErrorViewModel ValidateUser(string email, string password)
    {
        UserProfileViewModel user = _userRepository.GetByEmail(email);
        if (user == null || user.isActive == false )
        {
            return new CustomErrorViewModel { Message = "Inactive", Status = false };
        }
        if(user.isLoggedIn==false){
            return new CustomErrorViewModel { Message = "FirstLogin", Status = false };
        }
        bool userPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
        if (userPassword == false)
        {
            return new CustomErrorViewModel { Message = "Password does not match", Status = false };
        }
        return new CustomErrorViewModel { Message = "LoggedIn", Status = true };

    }
    public List<string> GetAllEmail()
    {
        return _userRepository.GetAllEmail();
    }
    public bool ValidateUserByEmail(string email)
    {
        UserProfileViewModel user = _userRepository.GetByEmail(email);
        if (user == null || user.isActive == false || user.Status == "InActive")
        {
            return false;
        }
        return true;
    }
    public UserProfileViewModel GetUserInfo(string email)
    {
        UserProfileViewModel user = _userRepository.GetByEmail(email);
        return user;
    }
    public (List<UserProfileViewModel>, int totalRecords) GetAllUser(string search, string sortOrder, int pageNumber, int pageSize)
    {
        int totalRecords;
        List<UserProfileViewModel> users = _userRepository.GetAllUser(search, sortOrder, pageNumber, pageSize, out totalRecords);
        return (users, totalRecords);
    }
    public string GetUserRole(string email)
    {
        return _userRepository.GetUserRole(email);
    }
    public bool DeleteUser(int id)
    {
        return _userRepository.DeleteUser(id);
    }
    public void ResetPassword(string newPassword, string email)
    {
        _userRepository.ResetPassword(newPassword, email);
    }
    public bool ChangePassword(string newPassword, string email, string currentPassword)
    {
        UserProfileViewModel user = _userRepository.GetByEmail(email);

        bool userPassword = BCrypt.Net.BCrypt.Verify(currentPassword, user.Password);

        if (userPassword || user.Password == currentPassword)
        {
            _userRepository.ResetPassword(newPassword, email);
            return true;
        }
        return false;
    }
    public bool UpdateProfile(UserProfileViewModel userProfileViewModel)
    {
        return _userRepository.UpdateProfile(userProfileViewModel);
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
    public List<Role> GetRoles(string token)
    {
        return _userRepository.GetRoles(token);
    }
    public bool AddUser(AddUserViewModel addUserViewModel)
    {
        string password = BCrypt.Net.BCrypt.HashPassword(addUserViewModel.Password);
        addUserViewModel.Password = password;
        return _userRepository.AddUser(addUserViewModel);
    }

}
