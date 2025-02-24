using DAL.Models;

namespace DAL.ViewModels;

public class UserProfileViewModel
{
     public int UserId { get; set; }
    public string FirstName { get; set; } 

    public string LastName { get; set; } 

    public string Email { get; set; }

    public string Phone { get; set; }
    public string Password { get; set; }
    public int? City { get; set; }

    public int? State { get; set; }

    public int? Country { get; set; }
    public string CountryName {get;set;}
    public string StateName {get;set;}
    public string CityName {get;set;}

    public string Address { get; set; }

    public string ZipCode { get; set; }

    public string ProfileImg { get; set; }

    public string UserName { get; set; }
    public string RoleName { get; set; }

    public bool isActive {get;set;}
    
    


}
