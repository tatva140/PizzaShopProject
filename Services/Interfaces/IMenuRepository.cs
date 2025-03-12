using System.Text.Json.Nodes;
using DAL.Models;
using DAL.ViewModels;

namespace Services.Interfaces;

public interface IMenuRepository
{
    List<Category> GetCategories();
    List<Item> GetCategoryItems(int id, int pageNumber, int pageSize, out int totalRecords);
    List<Modifier> GetModifiers(int id, int pageNumber, int pageSize, out int totalRecords);
    int AddCategory(Category category);
    int AddModifier(Modifier modifier);
    bool EditCategory(Category category);
    Category CategoryDetails(int id);
    bool DeleteCategory(int id);
    bool DeleteItem(int id);
    bool DeleteModifier(int id);
    bool DeleteItems(JsonArray ids);
    bool DeleteModifiers(JsonArray ids);
    List<ModifierGroup> GetModifierGroups();
    int AddItem(MenuItemsViewModel menuItemsViewModel);
    Modifier ModifierDetails(int id);
    int EditModifier(Modifier modifier);
    int AddModifierGroup(JsonObject obj);
    int EditModifierGroup(MenuModifiersViewModel menuModifiersViewModel);
    bool DeleteModifierGroup(int id);
    ModifierGroup GetModifierGroupDetails(int id);
    List<Modifier> GetAllModifiers(int id);
    EditItemViewModel FetchItemDetails(int id);
    List<ModifierGroup> GetMGDetails(int id);
    List<int> GetMinMax(int id,int itemId);
    List<Modifier> GetModifiersForItemEdit(int modifierId);
    int EditItem(MenuItemsViewModel menuItemsViewModel);
}
