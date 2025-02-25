using System.ComponentModel.DataAnnotations;

namespace DAL.ViewModels;

public class AddUserViewModel
{
    public int UserId { get; set; }
    public string FirstName { get; set; } 

    public string LastName { get; set; } 

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
    public string Password { get; set; }

    public bool isActive {get;set;}
    
}
