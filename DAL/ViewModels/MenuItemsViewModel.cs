using DAL.Models;

namespace DAL.ViewModels;

public class MenuItemsViewModel
{
    public List<Category> categories {get;set;}
    public List<Item> items {get;set;}
    public List<ModifierGroup> modifierGroups {get;set;}

    public Category category {get;set;}
    public Item item {get;set;}

    public string CategoryName {get;set;}
    public int PageNumber {get;set;}
    public int PageSize {get;set;}
    public int TotalPages {get;set;}
    public int SelectedPage {get;set;}

}
