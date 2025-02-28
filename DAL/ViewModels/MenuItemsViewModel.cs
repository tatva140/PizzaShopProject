using DAL.Models;

namespace DAL.ViewModels;

public class MenuItemsViewModel
{
    public List<Category> categories {get;set;}
    public List<Item> items {get;set;}

    public Category category {get;set;}

}
