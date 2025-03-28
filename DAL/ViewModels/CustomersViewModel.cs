namespace DAL.ViewModels;

public class CustomersViewModel
{
   public List<CustomerListViewModel> Customers { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int SelectedPage { get; set; }
    public string CustomerName {get;set;}
    public string OrderType {get;set;}
    public string Payment {get;set;}
    public string Phone {get;set;}
    public DateTime comingSince {get;set;}
    public DateTime OrderDate {get;set;}
    public int itemsCount {get;set;}
    public decimal Amount {get;set;}
}
