using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace DAL.ViewModels;

public class UserProfileViewModel
{
    public int UserId { get; set; }

[Required]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Enter Valid First Name.")]
    public string FirstName { get; set; }

   [Required]
    [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Enter Valid Last Name.")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is Required")]
    public string? Email { get; set; }

    [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Mobile Number.")]
    public string? Phone { get; set; }
    public string? Password { get; set; }
    public string? Status { get; set; }
    public int? City { get; set; }
    public int? State { get; set; }
    public int? Country { get; set; }
    public string? CountryName { get; set; }
    public string? StateName { get; set; }
    public string? CityName { get; set; }
    public string? Address { get; set; }

    [RegularExpression(@"^[0-9]{6}$", ErrorMessage = "Invalid ZipCode")]
    public string? ZipCode { get; set; }
    public string? ProfileImg { get; set; }

  
    public string? UserName { get; set; }
    public string? RoleName { get; set; }
    public int RoleId { get; set; }
    public bool isActive { get; set; }
}