using DAL.Models;

namespace DAL.ViewModels;

public class OrderAppMenuViewModel
{
    public List<Category> categories {get;set;}
    public List<Item> items {get;set;}
}
