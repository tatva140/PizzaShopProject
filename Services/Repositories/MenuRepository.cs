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

    public List<Item> GetCategoryItems(int id, int pageNumber, int pageSize, out int totalRecords)
    {
        //   if (id == 0)
        // {
        //     IQueryable<Item> items1 = _context.Items.Where(i =>  i.IsActive == true).OrderBy(i => i.CreatedAt);
        //     totalRecords = items1.Count();
        //     return items1.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        // }
        IQueryable<Item> items = _context.Items.Where(i => i.CategoryId == id && i.IsActive == true).OrderBy(i => i.CreatedAt);
        totalRecords = items.Count();
        return items.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }
    public List<Modifier> GetModifiers(int id, int pageNumber, int pageSize, out int totalRecords)
    {
        if (id == 0)
        {
            IQueryable<Modifier> allModifiers=_context.Modifiers.Where(m=>m.IsActive==true);
            totalRecords = allModifiers.Count();
            return allModifiers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
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

    if(menuItemsViewModel.selectedModifierList!=null){
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
        return item.CategoryId??0;
    }

    public int AddModifier(Modifier modifier)
    {

        if (modifier.ModifierName == null || modifier.ModifierGroupId == null || modifier.Unit == null || modifier.Rate == 0 || modifier.Quantity == 0)
        {
            return 0;
        }

        _context.Modifiers.Add(modifier);
        _context.SaveChanges();

        ModifierModifierGroup modifierModifierGroup = new ModifierModifierGroup
        {
            ModifierGroupId = modifier.ModifierGroupId ?? 0,
            ModifierId = modifier.ModifierId

        };
        _context.ModifierModifierGroups.Add(modifierModifierGroup);
        _context.SaveChanges();
        return modifier.ModifierGroupId??0;
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
    public Modifier ModifierDetails(int id)
    {
        return _context.Modifiers.Find(id);
    }

    public int EditModifier(Modifier modifier)
    {
       
        if (modifier.ModifierName == null || modifier.ModifierGroupId == null || modifier.Unit == null || modifier.Rate == 0 || modifier.Quantity == 0)
        {
            return 0;
        }
        Modifier modifier1 = _context.Modifiers.FirstOrDefault(c => c.ModifierId == modifier.ModifierId && c.IsActive == true);
        if (modifier1 == null) return 0;
        modifier1.Description = modifier.Description ?? modifier1.Description;
        modifier1.ModifierName = modifier.ModifierName ?? modifier1.ModifierName;
        modifier1.Rate = modifier.Rate == 0 ? modifier1.Rate : modifier.Rate;
        modifier1.Quantity = modifier.Quantity == 0 ? modifier1.Quantity : modifier.Quantity;
        modifier1.Unit = modifier.Unit ?? modifier1.Unit;

        ModifierModifierGroup modifierModifierGroup = _context.ModifierModifierGroups.FirstOrDefault(mg => mg.ModifierGroupId == modifier1.ModifierGroupId && mg.ModifierId == modifier.ModifierId);
        modifierModifierGroup.ModifierGroupId=modifier.ModifierGroupId??0;


        _context.SaveChanges();
        return modifier.ModifierGroupId??0;
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
            ModifierModifierGroup modifierModifierGroup2 = _context.ModifierModifierGroups.FirstOrDefault(mg => mg.ModifierGroupId == menuModifiersViewModel.modifierGroup.ModifierGroupId);
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
}
