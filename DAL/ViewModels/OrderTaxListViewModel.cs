namespace DAL.ViewModels;

public class OrderTaxListViewModel
{
    public string TaxName { get; set; }
    public decimal? Rate { get; set; }
    public decimal? Amount { get; set; }
    public int OrderId { get; set; }
}
