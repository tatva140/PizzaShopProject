namespace DAL.ViewModels;

public class UserInfoViewModel
{
    public int UserId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string Country { get; set; }

    public string Address { get; set; }

    public string ZipCode { get; set; }

    public string ProfileImg { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public int UpdatedBy { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string ResetCode { get; set; }

    public int RoleId { get; set; }
}
