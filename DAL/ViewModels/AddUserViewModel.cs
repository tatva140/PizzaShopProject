namespace DAL.ViewModels;

public class AddUserViewModel
{
    public int UserId { get; set; }
    public string FirstName { get; set; } 

    public string LastName { get; set; } 

    public string Email { get; set; }

    public string? Phone { get; set; }
    public int? City { get; set; }

    public int? State { get; set; }

    public int? Country { get; set; }

    public string? Address { get; set; }

    public string? ZipCode { get; set; }

    public string? ProfileImg { get; set; }

    public string UserName { get; set; }
    public int RoleId { get; set; }
    public string Password { get; set; }

    public bool isActive {get;set;}
    
}
