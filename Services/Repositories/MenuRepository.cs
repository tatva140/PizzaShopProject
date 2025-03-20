using System.ComponentModel;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Nodes;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Services.Interfaces;

namespace Services.Repositories;

public class MenuRepository : IMenuRepository
{
    private readonly PizzashopContext _context;

    public MenuRepository(PizzashopContext context)
    {
        _context = context;
    }

    public List<Category> GetCategories()
    {
        return _context.Categories.Where(c => c.IsActive == true).ToList();
    }

    public List<Item> GetCategoryItems(int id, string search, int pageNumber, int pageSize, out int totalRecords)
    {
        if (search != null)
        {
            IQueryable<Item> items1 = _context.Items.Where(i => i.CategoryId == id && i.IsActive == true && i.Name.ToLower().Contains(search.ToLower())).AsQueryable();
            totalRecords = items1.Count();
            return items1.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        IQueryable<Item> items = _context.Items.Where(i => i.CategoryId == id && i.IsActive == true).OrderBy(i => i.CreatedAt);
        totalRecords = items.Count();
        return items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }
    public List<Modifier> GetModifiers(int id, string search, int pageNumber, int pageSize, out int totalRecords)
    {
        if (id == 0)
        {
            if (search != null)
            {
                IQueryable<Modifier> allModifiers1 = _context.Modifiers.Where(m => m.IsActive == true && m.ModifierName.ToLower().Contains(search.ToLower()));
                totalRecords = allModifiers1.Count();
                return allModifiers1.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
            IQueryable<Modifier> allModifiers = _context.Modifiers.Where(m => m.IsActive == true);
            totalRecords = allModifiers.Count();
            return allModifiers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        if (search != null)
        {
            List<ModifierModifierGroup> modifierModifierGroups1 = _context.ModifierModifierGroups.Where(i => i.ModifierGroupId == id).ToList();
            IQueryable<Modifier> modifiers1 = (from mmg in _context.ModifierModifierGroups
                                               join m in _context.Modifiers
                                               on mmg.ModifierId equals m.ModifierId
                                               where mmg.ModifierGroupId == id
                                               select m);
            IQueryable<Modifier> allModifiers1 = modifiers1.Where(m => m.IsActive == true && m.ModifierName.ToLower().Contains(search.ToLower()));

            totalRecords = allModifiers1.Count();
            return allModifiers1.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        List<ModifierModifierGroup> modifierModifierGroups = _context.ModifierModifierGroups.Where(i => i.ModifierGroupId == id).ToList();
        IQueryable<Modifier> modifiers = (from mmg in _context.ModifierModifierGroups
                                          join m in _context.Modifiers
                                          on mmg.ModifierId equals m.ModifierId
                                          where mmg.ModifierGroupId == id
                                          select m);


        totalRecords = modifiers.Count();
        return modifiers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }


    public int AddCategory(Category category)
    {
        Category category1 = _context.Categories.FirstOrDefault(c => c.Name == category.Name && c.IsActive == true);
        if (category1 != null) return 0;
        _context.Categories.Add(category);
        _context.SaveChanges();
        return category.CategoryId;
    }
    public bool EditCategory(Category category)
    {
        Category category1 = _context.Categories.FirstOrDefault(c => c.CategoryId == category.CategoryId);
        if (category1 == null) return false;
        category1.IsActive = true;
        category1.Description = category.Description ?? category1.Description;
        category1.Name = category.Name ?? category1.Name;
        _context.SaveChanges();
        return true;
    }

    public Category CategoryDetails(int id)
    {
        return _context.Categories.Find(id);
    }

    public bool DeleteCategory(int id)
    {
        Category category = _context.Categories.FirstOrDefault(u => u.CategoryId == id && u.IsActive == true);
        if (category == null) return false;
        category.IsActive = false;

        List<Item> items = _context.Items.Where(c => c.CategoryId == id).ToList();
        foreach (var item in items)
        {
            item.IsActive = false;
        }

        _context.SaveChanges();
        return true;
    }

    public bool DeleteItem(int id)
    {
        Item item = _context.Items.FirstOrDefault(i => i.ItemId == id && i.IsActive == true);
        if (item == null) return false;
        item.IsActive = false;
        // _context.Items.Update(item);
        _context.SaveChanges();
        return true;
    }
    public bool DeleteItems(JsonArray ids)
    {
        if (ids.Count == 0) return false;
        foreach (int itemId in ids)
        {
            Item item = _context.Items.FirstOrDefault(i => i.ItemId == itemId && i.IsActive == true);
            if (item == null) return false;
            item.IsActive = false;
        }
        _context.SaveChanges();
        return true;
    }

    public List<ModifierGroup> GetModifierGroups()
    {
        return _context.ModifierGroups.Where(m => m.IsActive == true).ToList();
    }

    public int AddItem(MenuItemsViewModel menuItemsViewModel)
    {

        if (menuItemsViewModel.Name == null || menuItemsViewModel.CategoryId == null || menuItemsViewModel.ItemType == null || menuItemsViewModel.Rate == 0 || menuItemsViewModel.Quantity == 0 || menuItemsViewModel.Unit == null)
        {
            return 0;
        }
        Item item = new Item
        {
            Name = menuItemsViewModel.Name,
            CategoryId = menuItemsViewModel.CategoryId,
            ItemType = menuItemsViewModel.ItemType,
            Rate = menuItemsViewModel.Rate,
            Quantity = menuItemsViewModel.Quantity,
            Unit = menuItemsViewModel.Unit,
            Description = menuItemsViewModel.Description ?? null,
            DefaultTax = menuItemsViewModel.DefaultTax,
            IsAvailable = menuItemsViewModel.IsAvailable,
            TaxPercentage = menuItemsViewModel.TaxPercentage,
            ItemImg = menuItemsViewModel.ItemImg ?? null,
        };
        _context.Items.Add(item);
        _context.SaveChanges();

        if (menuItemsViewModel.selectedModifierList != null)
        {
            foreach (int modifier in menuItemsViewModel.selectedModifierList)
            {
                int min = 0;
                int max = 0;
                foreach (var key in menuItemsViewModel.dropdownSelections)
                {
                    if (key.Index == modifier)
                    {
                        min = key.Min;
                        max = key.Max;
                        break;
                    }
                }
                ModifierGroupsItem modifierGroupsItem = new ModifierGroupsItem
                {
                    ModifierGroupId = modifier,
                    ItemId = item.ItemId,
                    Min = min,
                    Max = max
                };
                _context.ModifierGroupsItems.Add(modifierGroupsItem);
            }
        }
        _context.SaveChanges();
        return item.CategoryId ?? 0;
    }

    public int AddModifier(Modifier modifier,List<int> ids)
    {
        if (modifier.ModifierName == null  || modifier.Unit == null || modifier.Rate == 0 || modifier.Quantity == 0)
        {
            return 0;
        }

        _context.Modifiers.Add(modifier);
        _context.SaveChanges();

        foreach (var id in ids)
        { 
        ModifierModifierGroup modifierModifierGroup = new ModifierModifierGroup
        {
            ModifierGroupId = id,
            ModifierId = modifier.ModifierId
        };
        _context.ModifierModifierGroups.Add(modifierModifierGroup);
        }
        _context.SaveChanges();
        return ids[0] ;
    }
    public bool DeleteModifier(int id)
    {
        Modifier modifier = _context.Modifiers.FirstOrDefault(i => i.ModifierId == id && i.IsActive == true);
        if (modifier == null) return false;
        modifier.IsActive = false;

        List<ModifierModifierGroup> modifierModifierGroup = _context.ModifierModifierGroups.Where(m => m.ModifierId == id).ToList();
        foreach (var modifiers in modifierModifierGroup)
        {
            _context.ModifierModifierGroups.Remove(modifiers);
        }
        // _context.Modifiers.Update(modifier);
        _context.SaveChanges();
        return true;
    }
    public bool DeleteModifiers(JsonArray ids)
    {
        if (ids.Count == 0) return false;
        foreach (int modifierId in ids)
        {
            Modifier modifier = _context.Modifiers.FirstOrDefault(i => i.ModifierId == modifierId && i.IsActive == true);
            if (modifier == null) return false;
            modifier.IsActive = false;

            List<ModifierModifierGroup> modifierModifierGroup = _context.ModifierModifierGroups.Where(m => m.ModifierId == modifierId).ToList();
            foreach (var modifiers in modifierModifierGroup)
            {
                _context.ModifierModifierGroups.Remove(modifiers);
            }
        }
        _context.SaveChanges();
        return true;
    }
    public MenuModifiersViewModel ModifierDetails(int id)
    {
        List<int> modifierModifierGroups=_context.ModifierModifierGroups.Where(m=>m.ModifierId==id).Select(m=>m.ModifierGroupId).ToList();
        Modifier modifier=_context.Modifiers.Find(id);
        MenuModifiersViewModel menuModifiersViewModel=new MenuModifiersViewModel{
            ids=modifierModifierGroups,
            ModifierName=modifier.ModifierName,
            Rate=modifier.Rate,
            Quantity=modifier.Quantity??0,
            Unit=modifier.Unit,
            Description=modifier.Description
        };
        return menuModifiersViewModel;
    }

    public int EditModifier(MenuModifiersViewModel menuModifiersViewModel)
    {

        Modifier modifier1 = _context.Modifiers.FirstOrDefault(c => c.ModifierId == menuModifiersViewModel.ModifierId && c.IsActive == true);
        if (modifier1 == null) return 0;
        modifier1.Description = menuModifiersViewModel.Description ?? modifier1.Description;
        modifier1.ModifierName = menuModifiersViewModel.ModifierName ?? modifier1.ModifierName;
        modifier1.Rate = menuModifiersViewModel.Rate == 0 ? modifier1.Rate : menuModifiersViewModel.Rate;
        modifier1.Quantity = menuModifiersViewModel.Quantity == 0 ? modifier1.Quantity : menuModifiersViewModel.Quantity;
        modifier1.Unit = menuModifiersViewModel.Unit ?? modifier1.Unit;

        List<ModifierModifierGroup> modifierModifierGroup = _context.ModifierModifierGroups.Where(mg => mg.ModifierId == menuModifiersViewModel.ModifierId).ToList();

        foreach (var modifiersGroup in modifierModifierGroup)
        {
            if (!menuModifiersViewModel.ids.Contains(modifiersGroup.ModifierGroupId))
            {
                _context.ModifierModifierGroups.Remove(modifiersGroup);
            }

        }
        foreach (int i in menuModifiersViewModel.ids)
        {
            ModifierModifierGroup modifierModifierGroup2 = _context.ModifierModifierGroups.FirstOrDefault(mg => mg.ModifierId == menuModifiersViewModel.ModifierId && mg.ModifierGroupId == i);
            if (modifierModifierGroup2 == null)
            {
                ModifierModifierGroup modifierModifierGroup1 = new ModifierModifierGroup
                {
                    ModifierGroupId = i,
                    ModifierId = menuModifiersViewModel.ModifierId
                };
                _context.ModifierModifierGroups.Add(modifierModifierGroup1);
            }

        }
       


        _context.SaveChanges();
        return 1;
    }

    public int AddModifierGroup(JsonObject obj)
    {
        string modifierGroupName = obj["modifierGroupName"].ToString();
        if (obj["modifierGroupName"].ToString() == null) return 0;
        string Description = obj["Description"].ToString();
        ModifierGroup modifierGroup = new ModifierGroup
        {
            ModifierGroupName = modifierGroupName,
            Description = Description
        };
        _context.ModifierGroups.Add(modifierGroup);
        _context.SaveChanges();


        JsonArray jsonArray = (JsonArray)obj["ids"];
        foreach (int i in jsonArray)
        {
            ModifierModifierGroup modifierModifierGroup = new ModifierModifierGroup
            {
                ModifierGroupId = modifierGroup.ModifierGroupId,
                ModifierId = i
            };

            _context.ModifierModifierGroups.Add(modifierModifierGroup);
        }
        _context.SaveChanges();
        return modifierGroup.ModifierGroupId;
    }
    public int EditModifierGroup(MenuModifiersViewModel menuModifiersViewModel)
    {
        ModifierGroup modifierGroup = _context.ModifierGroups.FirstOrDefault(mg => mg.ModifierGroupId == menuModifiersViewModel.modifierGroup.ModifierGroupId);
        if (modifierGroup == null) return 0;
        modifierGroup.ModifierGroupName = menuModifiersViewModel.modifierGroup.ModifierGroupName;
        modifierGroup.Description = menuModifiersViewModel.modifierGroup.Description ?? "";



        List<ModifierModifierGroup> modifierModifierGroup = _context.ModifierModifierGroups.Where(mg => mg.ModifierGroupId == menuModifiersViewModel.modifierGroup.ModifierGroupId).ToList();

        foreach (var modifiersGroup in modifierModifierGroup)
        {
            if (!menuModifiersViewModel.ids.Contains(modifiersGroup.ModifierId))
            {
                _context.ModifierModifierGroups.Remove(modifiersGroup);
            }

        }
        foreach (int i in menuModifiersViewModel.ids)
        {
            ModifierModifierGroup modifierModifierGroup2 = _context.ModifierModifierGroups.FirstOrDefault(mg => mg.ModifierGroupId == menuModifiersViewModel.modifierGroup.ModifierGroupId && mg.ModifierId == i);
            if (modifierModifierGroup2 == null)
            {
                ModifierModifierGroup modifierModifierGroup1 = new ModifierModifierGroup
                {
                    ModifierGroupId = menuModifiersViewModel.modifierGroup.ModifierGroupId,
                    ModifierId = i
                };
                _context.ModifierModifierGroups.Add(modifierModifierGroup1);
            }

        }

        _context.SaveChanges();
        return modifierGroup.ModifierGroupId;
    }

    public bool DeleteModifierGroup(int id)
    {
        ModifierGroup modifierGroup = _context.ModifierGroups.FirstOrDefault(u => u.ModifierGroupId == id && u.IsActive == true);
        if (modifierGroup == null) return false;
        modifierGroup.IsActive = false;

        List<ModifierModifierGroup> modifierModifierGroups = _context.ModifierModifierGroups.Where(c => c.ModifierGroupId == id).ToList();
        foreach (var modifier in modifierModifierGroups)
        {
            _context.ModifierModifierGroups.Remove(modifier);
        }

        _context.SaveChanges();
        return true;
    }

    public ModifierGroup GetModifierGroupDetails(int id)
    {
        return _context.ModifierGroups.Find(id);
    }
    public List<Modifier> GetAllModifiers(int id)
    {
        List<ModifierModifierGroup> modifierModifierGroups = _context.ModifierModifierGroups.Where(i => i.ModifierGroupId == id).ToList();
        List<Modifier> modifiers = (from mmg in _context.ModifierModifierGroups
                                    join m in _context.Modifiers
                                    on mmg.ModifierId equals m.ModifierId
                                    where mmg.ModifierGroupId == id
                                    select m).ToList();
        return modifiers;
    }

    public EditItemViewModel FetchItemDetails(int id)
    {
        List<Category> categories = _context.Categories.Where(c => c.IsActive == true).ToList();
        Item item = _context.Items.FirstOrDefault(i => i.ItemId == id);
        if (item == null) return null;

        EditItemViewModel editItemViewModel = new EditItemViewModel
        {
            ItemId = id,
            Name = item.Name,
            ItemType = item.ItemType,
            CategoryId = item.CategoryId,
            Rate = item.Rate,
            Quantity = item.Quantity,
            Unit = item.Unit,
            IsAvailable = item.IsAvailable,
            DefaultTax = item.DefaultTax,
            Shortcode = item.Shortcode,
            TaxPercentage = item.TaxPercentage,
            Description = item.Description,
            ItemImg = item.ItemImg,
            categories = categories
        };
        return editItemViewModel;

    }
    public List<ModifierGroup> GetMGDetails(int id)
    {
        List<ModifierGroupsItem> modifierGroupsItems = _context.ModifierGroupsItems.Where(m => m.ItemId == id).ToList();
        List<ModifierGroup> modifierGroups = (from mmg in _context.ModifierGroupsItems
                                              join m in _context.ModifierGroups
                                              on mmg.ModifierGroupId equals m.ModifierGroupId
                                              where mmg.ItemId == id
                                              select m).ToList();
        return modifierGroups;
    }
    public List<int> GetMinMax(int id, int itemId)
    {
        List<ModifierModifierGroup> modifierModifierGroups = _context.ModifierModifierGroups.Where(i => i.ModifierGroupId == id).ToList();
        List<Modifier> modifiers = (from mmg in _context.ModifierModifierGroups
                                    join m in _context.Modifiers
                                    on mmg.ModifierId equals m.ModifierId
                                    where mmg.ModifierGroupId == id
                                    select m).ToList();
        ModifierGroupsItem modifierGroupsItems = _context.ModifierGroupsItems.FirstOrDefault(m => m.ItemId == itemId && m.ModifierGroupId == id);
        List<int> minmax = new List<int>
        {
            modifierGroupsItems.Min ?? 0,
            modifierGroupsItems.Max ?? 0,
            modifiers.Count
        };
        return minmax;
    }
    public List<Modifier> GetModifiersForItemEdit(int modifierId)
    {
        List<ModifierModifierGroup> modifierModifierGroups = _context.ModifierModifierGroups.Where(i => i.ModifierGroupId == modifierId).ToList();
        List<Modifier> modifiers = (from mmg in _context.ModifierModifierGroups
                                    join m in _context.Modifiers
                                    on mmg.ModifierId equals m.ModifierId
                                    where mmg.ModifierGroupId == modifierId
                                    select m).ToList();
        return modifiers;
    }

    public int EditItem(MenuItemsViewModel menuItemsViewModel)
    {

        if (menuItemsViewModel.Name == null || menuItemsViewModel.CategoryId == null || menuItemsViewModel.ItemType == null || menuItemsViewModel.Rate == 0 || menuItemsViewModel.Quantity == 0 || menuItemsViewModel.Unit == null)
        {
            return 0;
        }
        Item item = _context.Items.FirstOrDefault(i => i.ItemId == menuItemsViewModel.ItemId);
        if (item == null) return 0;

        item.Name = menuItemsViewModel.Name;
        item.CategoryId = menuItemsViewModel.CategoryId;
        item.ItemType = menuItemsViewModel.ItemType;
        item.Rate = menuItemsViewModel.Rate;
        item.Quantity = menuItemsViewModel.Quantity;
        item.Unit = menuItemsViewModel.Unit;
        item.Description = menuItemsViewModel.Description ?? item.Description;
        item.DefaultTax = menuItemsViewModel.DefaultTax;
        item.IsAvailable = menuItemsViewModel.IsAvailable;
        item.TaxPercentage = menuItemsViewModel.TaxPercentage;
        item.ItemImg = menuItemsViewModel.ItemImg ?? item.ItemImg;


        if (menuItemsViewModel.selectedModifierList != null)
        {
            foreach (int modifier in menuItemsViewModel.selectedModifierList)
            {
                ModifierGroupsItem modifierGroupsItem1 = _context.ModifierGroupsItems.FirstOrDefault(mmg => mmg.ModifierGroupId == modifier && mmg.ItemId == item.ItemId);


                int min = 0;
                int max = 0;
                foreach (var key in menuItemsViewModel.dropdownSelections)
                {
                    if (key.Index == modifier)
                    {
                        min = key.Min;
                        max = key.Max;
                        break;
                    }
                }
                if (modifierGroupsItem1 == null)
                {
                    ModifierGroupsItem modifierGroupsItem = new ModifierGroupsItem
                    {
                        ModifierGroupId = modifier,
                        ItemId = item.ItemId,
                        Min = min,
                        Max = max
                    };
                    _context.ModifierGroupsItems.Add(modifierGroupsItem);
                }
                else
                {
                    modifierGroupsItem1.Min = min;
                    modifierGroupsItem1.Max = max;
                }
            }
        }
        _context.SaveChanges();
        return item.CategoryId ?? 0;
    }
}


