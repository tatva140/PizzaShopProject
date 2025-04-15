namespace DAL.ViewModels;

public class OrderItemListViewModel
{
    public int OrderItemsId { get; set; }

    public int? OrderId { get; set; }

    public int? ItemId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Rate { get; set; }
    public string ItemName { get; set; }
    public List<OrderModifierListViewModel> modifierLists { get; set; }

}