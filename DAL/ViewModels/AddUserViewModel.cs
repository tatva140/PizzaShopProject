using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DAL.ViewModels;

public class AddUserViewModel
{
    public int UserId { get; set; }

    [Required]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Enter Valid First Name.")]
    public string FirstName { get; set; }

    [Required]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Enter Valid Last Name.")]
    public string LastName { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is Required")]
    [Remote("CheckUserExists","User",ErrorMessage="Email Already Exists.")]
    public string Email { get; set; }

    [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Mobile Number.")]
    public string? Phone { get; set; }
    public int? City { get; set; }

    public int? State { get; set; }

    public int? Country { get; set; }

    public string? Address { get; set; }

    [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Invalid ZipCode")]
    public string? ZipCode { get; set; }

    public string? ProfileImg { get; set; }

    public string UserName { get; set; }
    public int RoleId { get; set; }
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password Is Required")]
    [RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_]).{8,}$", ErrorMessage = "Please enter a password of minimum length 8 with atleast one special character and  one uppercase letter.")]
    public string Password { get; set; }

    public bool isActive { get; set; }

}
