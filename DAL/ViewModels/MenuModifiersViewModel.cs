using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace DAL.ViewModels;

public class MenuModifiersViewModel
{
     public List<Modifier> modifier {get;set;}
    public ModifierGroup modifierGroup {get;set;}
    public List<ModifierGroup> modifierGroups {get;set;}

    public int PageNumber {get;set;}
    public int PageSize {get;set;}
    public int TotalPages {get;set;}
    public int SelectedPage {get;set;}

    public int ModifierId { get; set; }

    [Required(ErrorMessage="Name is Required")]
    public string ModifierName { get; set; }

    public string? Description { get; set; }

    public int? ModifierGroupId { get; set; }

    [RegularExpression(@"^([1-9]\d*|\d+\.\d+)$", ErrorMessage = "Invalid Rate.")]
    public decimal Rate { get; set; }

    [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Invalid Quantity.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage="Unit is Required")]
    public string Unit { get; set; }

    public bool? IsActive { get; set; }

}
