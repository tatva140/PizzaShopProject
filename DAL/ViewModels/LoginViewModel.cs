namespace DAL.ViewModels;
using System.ComponentModel.DataAnnotations;

public class LoginViewModel
{
    [EmailAddress]
    [Required(ErrorMessage="Email is Required")]
    public string Email{get;set;}

    [DataType(DataType.Password)]
    [Required(ErrorMessage="Password Is Required")]
    public string Password {get;set;}

    public bool RememberMe {get; set;}

}
