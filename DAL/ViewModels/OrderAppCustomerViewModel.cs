using System.ComponentModel.DataAnnotations;

namespace DAL.ViewModels;

public class OrderAppCustomerViewModel
{
    [Required]
    [RegularExpression(@"^(?=.*[A-Za-z]).*$", ErrorMessage = "Must contain at least one letter.")]
    public string FirstName {get;set;}
    

    [EmailAddress]
    [Required(ErrorMessage = "Email is Required")]
    public string EmailAddress {get;set;}

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Value must be at least 1.")]
    public int NoOfPersons {get;set;}

    public int SectionId {get;set;}

    [Required]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid Mobile Number.")]
    public string Phone {get;set;}

    public List<int> selectedTables {get;set;}

    public int waitingTokenId {get;set;}

    public bool EditFlag {get;set;}

    public string SectionName {get;set;}



}
