using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Http.Internal;
using Services.Interfaces;
using static DAL.ViewModels.OrderAppMenuViewModel;

namespace Services.Repositories;

public class OrderAppMenuRepository : IOrderAppMenuRepository
{
    private readonly PizzashopContext _context;
    public OrderAppMenuRepository(PizzashopContext context)
    {
        _context = context;
    }
    public List<Item> GetCategoryItems(int id)
    {
        List<Item> items = new List<Item>();
        if (id == -1)
        {
            items = _context.Items.Where(i => i.Isfavourite == true && i.IsActive == true && i.IsAvailable==true).ToList();
        }
        else if (id == 0)
        {

            items = _context.Items.Where(i => i.IsActive == true && i.IsAvailable==true).ToList();
        }
        else
        {

            items = _context.Items.Where(i => i.CategoryId == id && i.IsActive == true && i.IsAvailable==true).ToList();
        }
        return items;
    }
    public Item GetItem(int id)
    {
        Item item = _context.Items.Where(i => i.ItemId == id && i.IsAvailable==true && i.IsActive==true).FirstOrDefault();
        return item;
    }
    public void Update(Item item)
    {
        _context.Items.Update(item);
        _context.SaveChanges();
    }

    public OrderAppMenuViewModel GetModifierDetails(int id)
    {
        List<ModifierGroupsItem> modifierGroupsItems = _context.ModifierGroupsItems.Where(m => m.ItemId == id).ToList();
        // List<ModifierGroup> modifierGroups = 
        OrderAppMenuViewModel orderAppMenuViewModel =
                                              new OrderAppMenuViewModel
                                              {
                                                  modifierGroupDetails = (from mmg in _context.ModifierGroupsItems
                                                                          join m in _context.ModifierGroups
                                                                          on mmg.ModifierGroupId equals m.ModifierGroupId
                                                                          
                                                                          where mmg.ItemId == id
                                                                          select new ModifierGroupDetails
                                                                          {
                                                                              ModifierGroupName = m.ModifierGroupName,
                                                                              ModifierGroupId=m.ModifierGroupId,
                                                                              Min = mmg.Min ?? 0,
                                                                              Max = mmg.Max ?? 0,
                                                                              modifiers = (from mg in _context.ModifierModifierGroups
                                                                                            join mod in _context.Modifiers on mg.ModifierId equals mod.ModifierId
                                                                                            where mmg.ModifierGroupId==mg.ModifierGroupId
                                                                                            select mod).ToList()
                                                                          }).ToList()

                                              };
                                              return orderAppMenuViewModel;
    }
    public List<Table> GetCustomerTables(int id)
    {
        List<Table> tables=_context.Tables.Where(t=>t.CurrentCustomerId==id).ToList();
        return tables;
    }
    public Section GetSection(int id){
        Section section=_context.Sections.Where(s=>s.SectionId==id).FirstOrDefault();
        return section;
    }
    public List<Tax> GetTax(){
        List<Tax> taxes=_context.Taxes.Where(t=>t.IsActive==true && t.IsEnabled==true).ToList();
        return taxes;
    }
}
