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
            IQueryable<Modifier> allmodifiers = _context.Modifiers.Where(i => i.IsActive == true).OrderBy(i => i.CreatedAt);
            totalRecords = allmodifiers.Count();
            return allmodifiers.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        }
        IQueryable<Modifier> modifiers = _context.Modifiers.Where(i => i.ModifierGroupId == id && i.IsActive == true).OrderBy(i => i.CreatedAt);
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
        _context.Categories.Update(category1);
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

        _context.Categories.Update(category);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteItem(int id)
    {
        Item item = _context.Items.FirstOrDefault(i => i.ItemId == id && i.IsActive == true);
        if (item == null) return false;
        item.IsActive = false;
        _context.Items.Update(item);
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

    public bool AddItem(MenuItemsViewModel menuItemsViewModel)
    {
        
        if (menuItemsViewModel.Name == null || menuItemsViewModel.CategoryId == null || menuItemsViewModel.ItemType == null || menuItemsViewModel.Rate == 0 || menuItemsViewModel.Quantity == 0 || menuItemsViewModel.Unit == null)
        {
            return false;
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
            ItemImg = menuItemsViewModel.ItemImg?? null ,
        };
        _context.Items.Add(item);
        _context.SaveChanges();
    
        
        foreach (int modifier in menuItemsViewModel.selectedModifierList)
        {
            int min=0;
            int max=0;
            foreach (var key in menuItemsViewModel.dropdownSelections)
            {
                if(key.Index==modifier){
                    min=key.Min;
                    max=key.Max;
                    break;
                }
            }
            ModifierGroupsItem modifierGroupsItem=new ModifierGroupsItem{
                ModifierGroupId=modifier,
                ItemId=item.ItemId,
                Min=min,
                Max=max
            };
            _context.ModifierGroupsItems.Add(modifierGroupsItem);
        }
        _context.SaveChanges();
        return true;
    }

    public bool AddModifier(Modifier modifier)
    {

        if (modifier.ModifierName == null || modifier.ModifierGroupId == null || modifier.Unit == null || modifier.Rate == 0 || modifier.Quantity == 0)
        {
            return false;
        }
        _context.Modifiers.Add(modifier);
        _context.SaveChanges();
        return true;
    }
    public bool DeleteModifier(int id)
    {
        Modifier modifier = _context.Modifiers.FirstOrDefault(i => i.ModifierId == id && i.IsActive == true);
        if (modifier == null) return false;
        modifier.IsActive = false;
        _context.Modifiers.Update(modifier);
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
        }
        _context.SaveChanges();
        return true;
    }
    public Modifier ModifierDetails(int id)
    {
        return _context.Modifiers.Find(id);
    }

    public bool EditModifier(int modifierId, string modifierGroupId, string modifierName, string unit, decimal rate, int quantity, string description)
    {
        if (modifierName == null || modifierGroupId == null || unit == null || rate == 0 || quantity == 0)
        {
            return false;
        }
        Modifier modifier1 = _context.Modifiers.FirstOrDefault(c => c.ModifierId == modifierId);
        modifier1.IsActive = true;
        modifier1.Description = description ?? modifier1.Description;
        modifier1.ModifierName = modifierName ?? modifier1.ModifierName;
        modifier1.Rate = rate == 0 ? modifier1.Rate : rate;
        modifier1.Quantity = quantity == 0 ? modifier1.Quantity : quantity;
        modifier1.Unit = unit ?? modifier1.Unit;
        _context.Modifiers.Update(modifier1);
        _context.SaveChanges();
        return true;
    }

    public int AddModifierGroup(JsonObject obj)
    {
        string modifierGroupName = obj["modifierGroupName"].ToString();
        if (obj["modifierGroupName"].ToString() == null ) return 0;
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
            Modifier modifier1 = _context.Modifiers.FirstOrDefault(m => m.ModifierId == i);
            Modifier modifier = new Modifier
            {
                ModifierName = modifier1.ModifierName,
                Description = modifier1.Description,
                Unit = modifier1.Unit,
                Rate = modifier1.Rate,
                Quantity = modifier1.Quantity,
                ModifierGroupId = modifierGroup.ModifierGroupId
            };
            _context.Modifiers.Add(modifier);
        }
        _context.SaveChanges();
        return modifierGroup.ModifierGroupId;
    }
    public int EditModifierGroup(JsonObject obj)
    {
        string modifierGroupName = obj["modifierGroupName"].ToString();
        if (obj["modifierGroupName"].ToString() == null ) return 0;
        string Description = obj["Description"].ToString();
        ModifierGroup modifierGroup = new ModifierGroup
        {
            ModifierGroupName = modifierGroupName,
            Description = Description??""
        };
        _context.ModifierGroups.Update(modifierGroup);
        _context.SaveChanges();

       
        JsonArray jsonArray = (JsonArray)obj["ids"];
        foreach (int i in jsonArray)
        {
            Modifier modifier1 = _context.Modifiers.FirstOrDefault(m => m.ModifierId == i && m.ModifierGroupId!=modifierGroup.ModifierGroupId);
            if(modifier1==null)break;
            Modifier modifier = new Modifier
            {
                ModifierName = modifier1.ModifierName,
                Description = modifier1.Description,
                Unit = modifier1.Unit,
                Rate = modifier1.Rate,
                Quantity = modifier1.Quantity,
                ModifierGroupId = modifierGroup.ModifierGroupId
            };
            _context.Modifiers.Add(modifier);
        }
        _context.SaveChanges();
        return modifierGroup.ModifierGroupId;
    }

    public bool DeleteModifierGroup(int id){
        ModifierGroup modifierGroup = _context.ModifierGroups.FirstOrDefault(u => u.ModifierGroupId == id && u.IsActive == true);
        if (modifierGroup == null) return false;
        modifierGroup.IsActive = false;

        List<Modifier> modifiers = _context.Modifiers.Where(c => c.ModifierGroupId == id).ToList();
        foreach (var modifier in modifiers)
        {
            modifier.IsActive = false;
        }

        _context.ModifierGroups.Update(modifierGroup);
        _context.SaveChanges();
        return true;
    }

    public ModifierGroup GetModifierGroupDetails(int id){
         return _context.ModifierGroups.Find(id);
    }
    public List<Modifier> GetAllModifiers(int id){
        return _context.Modifiers.Where(m=>m.ModifierGroupId==id && m.IsActive==true).ToList();
    }
}
