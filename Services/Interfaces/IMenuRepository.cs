using System.Text.Json.Nodes;
using DAL.Models;
using DAL.ViewModels;

namespace Services.Interfaces;

public interface IMenuRepository
{
    List<Category> GetCategories();
    List<Item> GetCategoryItems(int id,int pageNumber,int pageSize,out int totalRecords);
    bool AddCategory(Category category);
    bool EditCategory(Category category);
    Category CategoryDetails(int id);
    bool DeleteCategory(int id);
    bool DeleteItem(int id);
    bool DeleteItems(JsonArray ids);
    List<ModifierGroup> GetModifierGroups(int id);
}
