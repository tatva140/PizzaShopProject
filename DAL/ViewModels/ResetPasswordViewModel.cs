namespace DAL.ViewModels;
using System.ComponentModel.DataAnnotations;

public class ResetPasswordViewModel
{
   
    [DataType(DataType.Password)]
    [Required(ErrorMessage="Password Is Required")]
    public string newPassword {get;set;}

    [DataType(DataType.Password)]
    [Required(ErrorMessage="Please Confirm the Password")]
    [Compare(nameof(newPassword), ErrorMessage = "Password mismatch")]
    public string confirmPassword {get;set;}


    [DataType(DataType.Password)]
    [Required(ErrorMessage="Currrent Password Is Required")]
    
    public string currentPassword {get;set;}

   
    public string Email{get;set;}
}
