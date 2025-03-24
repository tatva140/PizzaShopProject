namespace DAL.ViewModels;

public class OrdersViewModel
{
    public List<OrdersListViewModel> Orders { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int SelectedPage { get; set; }
    public int TotalRecords { get; set; }
    
}
