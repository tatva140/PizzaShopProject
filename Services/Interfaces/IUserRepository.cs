namespace Services.Interfaces;
using DAL.Models;
using DAL.ViewModels;

public interface IUserRepository
{
    UserProfileViewModel GetByEmail(string email);
    void ResetPassword(string newPassword, string email);
    string GetUserRole( string email);
    List<Role> GetRoles( );
    
    List<UserProfileViewModel> GetAllUser(string sortOrder,int pageNumber,int pageSize,out int totalRecords);
    bool DeleteUser(int id);
    bool UpdateProfile(UserProfileViewModel userProfileViewModel);

    List<Country> GetCountries();
    List<State> GetStateByCountry(int countryId);
    List<City> GetCityByState(int stateId);

    bool AddUser(AddUserViewModel addUserViewModel);
    void SaveProfileImage(string filename,int id);

}

