using DAL.Models;

namespace DAL.ViewModels;

public class OrderAppMenuViewModel
{
    public int SectionId {get;set;}
    public string SectionName {get;set;}
    public List<Table> tables {get;set;}

    public List<Category> categories {get;set;}
    public List<Item> items {get;set;}
    public List<int> modifierList {get;set;}
    public int id {get;set;}

    public List<ModifierGroupDetails> modifierGroupDetails {get;set;}

    public List<Tax> taxes {get;set;}
    public class ModifierGroupDetails
    {
        public string ModifierGroupName {get;set;}
        public List<Modifier> modifiers {get;set;}
        public int Min {get;set;}
        public int Max {get;set;}

    }
    public OrdersListViewModel ordersListViewModel {get;set;}
}
