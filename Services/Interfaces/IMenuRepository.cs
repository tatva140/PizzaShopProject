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
    bool AddModifier(Modifier modifier);
    bool EditCategory(Category category);
    Category CategoryDetails(int id);
    bool DeleteCategory(int id);
    bool DeleteItem(int id);
    bool DeleteModifier(int id);
    bool DeleteItems(JsonArray ids);
    bool DeleteModifiers(JsonArray ids);
    List<ModifierGroup> GetModifierGroups();
    bool AddItem(MenuItemsViewModel menuItemsViewModel);
    Modifier ModifierDetails(int id);
    bool EditModifier(int modifierId, string modifierGroupId, string modifierName, string unit, decimal rate, int quantity, string description);
    int AddModifierGroup(JsonObject obj);
    int EditModifierGroup(JsonObject obj);
    bool DeleteModifierGroup(int id);
     ModifierGroup GetModifierGroupDetails(int id);
      List<Modifier> GetAllModifiers(int id);

}
