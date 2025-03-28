namespace DAL.ViewModels;

public class CustomerListViewModel
{
     public string CustomerFirstName { get; set; }
    public string CustomerLastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public int TotalOrders { get; set; }
    public DateTime Date { get; set; }
     public int CustomerId { get; set; }
    
}
