using DAL.Models;
using DAL.ViewModels;

namespace Services.Interfaces;

public interface IOrderAppMenuRepository
{
    List<Item> GetCategoryItems(int id);

    Item GetItem(int id);
    void Update(Item item);
    OrderAppMenuViewModel GetModifierDetails(int id);

    List<Table> GetCustomerTables(int id);
    Section GetSection(int id);
    List<Tax> GetTax();
}
