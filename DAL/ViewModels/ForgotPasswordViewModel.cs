namespace DAL.ViewModels;
using System.ComponentModel.DataAnnotations;


public class ForgotPasswordViewModel
{
    [Required(ErrorMessage="Email is Required")]
    [EmailAddress]
    public string Email{get;set;}
}
