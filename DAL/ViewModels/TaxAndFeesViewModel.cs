using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace DAL.ViewModels;

public class TaxAndFeesViewModel
{
    public List<Tax> taxes {get;set;}
     public int TaxId { get; set; }

[Required]
    [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Enter Valid  Name.")]
    public string? TaxName { get; set; }

[Required]
    [RegularExpression(@"^(?=.*[\d])[0-9\W]+$", ErrorMessage = "Enter Valid  Amount.")]
    public string Amount { get; set; }

    public string? Type { get; set; }

    public bool? IsActive { get; set; }

    public bool IsEnabled { get; set; }
     public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int SelectedPage { get; set; }

}
