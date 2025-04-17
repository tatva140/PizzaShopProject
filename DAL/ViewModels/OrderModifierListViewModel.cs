namespace DAL.ViewModels;

public class OrderModifierListViewModel
{
    public string ModifierName {get;set;}

    public int ModifierId    { get; set; }
    public int? Quantity { get; set; }
    public decimal? Rate { get; set; }

    public int ItemId { get; set; }
}
