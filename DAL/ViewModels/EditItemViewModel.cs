using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace DAL.ViewModels;

public class EditItemViewModel
{
    public List<Category> categories {get;set;}
    public List<ModifierGroup> modifierGroups {get;set;}

    public string CategoryName {get;set;}
    public int ItemId { get; set; }

    [Required(ErrorMessage="Name is Required")]
    public string Name { get; set; } = null!;

    [RegularExpression(@"^([1-9]\d*|\d+\.\d+)$", ErrorMessage = "Invalid Rate.")]
    public decimal Rate { get; set; }

    [RegularExpression(@"^[1-9]\d*$", ErrorMessage = "Invalid Quantity.")]
    public int Quantity { get; set; }

    [Required(ErrorMessage="Unit is Required")]
    public string Unit { get; set; }

    public bool IsAvailable { get; set; }

    public string? Description { get; set; }

    public string? ItemImg { get; set; }

    [RegularExpression(@"^([1-9]\d*|\d+\.\d+)$", ErrorMessage = "Invalid TaxPercentage.")]
    public decimal? TaxPercentage { get; set; }
    public bool DefaultTax { get; set; }

    public int? CategoryId { get; set; }

    public bool IsActive { get; set; }

    public bool? Isfavourite { get; set; }

    public string? Shortcode { get; set; }

    public string ItemType { get; set; }

    public List<ModifierGroup> moidfierGroups {get;set;}

    

}
