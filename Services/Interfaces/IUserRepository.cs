namespace Services.Interfaces;
using DAL.Models;
using DAL.ViewModels;

public interface IUserRepository
{
    void SetRememberMe(string email, bool RememberMe);
    UserProfileViewModel GetByEmail(string email);
    void ResetPassword(string newPassword, string email);
    string GetUserRole(string email);
    List<Role> GetRoles(string token);

    List<UserProfileViewModel> GetAllUser(string search, string sortOrder, int pageNumber, int pageSize, out int totalRecords);
    bool DeleteUser(int id);
    bool UpdateProfile(UserProfileViewModel userProfileViewModel);

    List<Country> GetCountries();
    List<State> GetStateByCountry(int countryId);
    List<City> GetCityByState(int stateId);

    bool AddUser(AddUserViewModel addUserViewModel);
    List<string> GetAllEmail();
}

