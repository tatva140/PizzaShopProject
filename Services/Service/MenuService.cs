using System.Text.Json.Nodes;
using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;
using Services.Repositories;

namespace Services.Service;

public class MenuService
{
    private readonly IMenuRepository _menuRepository;

    public MenuService(IMenuRepository menuRepository){
        _menuRepository=menuRepository;
    }
    public List<Category> GetCategories(){
       return _menuRepository.GetCategories();
    }

    public (List<Item>,int totalRecords) GetCategoryItems(int id,string search,int pageNumber,int pageSize){
        int totalRecords;
        List<Item> items=_menuRepository.GetCategoryItems(id,search,pageNumber,pageSize,out totalRecords);
        return  (items,totalRecords);
    }
    public (List<Modifier>,int totalRecords) GetModifiers(int id,string search,int pageNumber,int pageSize){
        int totalRecords;
        List<Modifier> modifiers=_menuRepository.GetModifiers(id,search,pageNumber,pageSize,out totalRecords);
        return  (modifiers,totalRecords);
    }

    public int AddCategory(Category category){
        return _menuRepository.AddCategory(category);
    }
    public int AddModifier(Modifier modifier){
        return _menuRepository.AddModifier(modifier);
    }
    public bool EditCategory(Category category){
        return _menuRepository.EditCategory(category);
    }

    public Category CategoryDetails(int id){
        return _menuRepository.CategoryDetails(id);
    }
    public Modifier ModifierDetails(int id){
        return _menuRepository.ModifierDetails(id);
    }

    public bool DeleteCategory(int id){
        return _menuRepository.DeleteCategory(id);
    }
    public List<ModifierGroup> GetModifierGroups(){
        return _menuRepository.GetModifierGroups();
    }
    public bool DeleteItem(int id){
        return _menuRepository.DeleteItem(id);
    }
    public bool DeleteItems(JsonArray ids){
        return _menuRepository.DeleteItems(ids);
    }
    public bool DeleteModifiers(JsonArray ids){
        return _menuRepository.DeleteModifiers(ids);
    }
    public bool DeleteModifier(int id){
        return _menuRepository.DeleteModifier(id);
    }

    public int AddItem(MenuItemsViewModel menuItemsViewModel){
        return _menuRepository.AddItem(menuItemsViewModel);
    }
     public int EditModifier(Modifier modifier){
        return _menuRepository.EditModifier(modifier);
    }
    public int AddModifierGroup(JsonObject obj){
        return _menuRepository.AddModifierGroup(obj);
    }
    public int EditModifierGroup(MenuModifiersViewModel menuModifiersViewModel){
        return _menuRepository.EditModifierGroup(menuModifiersViewModel);
    }
    public bool DeleteModifierGroup(int id){
        return _menuRepository.DeleteModifierGroup(id);
    }
     public ModifierGroup GetModifierGroupDetails( int id){
       return _menuRepository.GetModifierGroupDetails(id);
    }
       public List<Modifier> GetAllModifiers(int id){
        return _menuRepository.GetAllModifiers(id);
    }

    public EditItemViewModel FetchItemDetails(int id){
        return _menuRepository.FetchItemDetails(id);
    }
    public List<ModifierGroup> GetMGDetails(int id){
        return _menuRepository.GetMGDetails(id);
    }
    
    public List<int> GetMinMax(int id,int itemId){
        return _menuRepository.GetMinMax(id,itemId);
    }
    public List<Modifier> GetModifiersForItemEdit(int modifierId){
        return _menuRepository.GetModifiersForItemEdit(modifierId);
    }
    public int EditItem(MenuItemsViewModel menuItemsViewModel){
        return _menuRepository.EditItem(menuItemsViewModel);
    }
}