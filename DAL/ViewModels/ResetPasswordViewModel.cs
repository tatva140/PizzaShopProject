namespace DAL.ViewModels;
using System.ComponentModel.DataAnnotations;

public class ResetPasswordViewModel
{
   
    [DataType(DataType.Password)]
    [Required(ErrorMessage="Password Is Required")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_]).{8,}$",ErrorMessage ="Please enter a password of minimum length 8 with atleast one special character and  one uppercase letter.")]
    public string newPassword {get;set;}

    [DataType(DataType.Password)]
    [Required(ErrorMessage="Please Confirm the Password")]
    [Compare(nameof(newPassword), ErrorMessage = "Password mismatch")]
    public string confirmPassword {get;set;}


    [DataType(DataType.Password)]
    [Required(ErrorMessage="Currrent Password Is Required")]
    [MinLength(8, ErrorMessage = "Enter Valid Password")]
    
    public string currentPassword {get;set;}

   
    public string Email{get;set;}
}
