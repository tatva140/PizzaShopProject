using System.ComponentModel;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Nodes;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Services.Interfaces;

namespace Services.Repositories;

public class MenuRepository:IMenuRepository
{
     private readonly PizzashopContext _context;

    public MenuRepository(PizzashopContext context)
    {
        _context = context;
    }

    public List<Category> GetCategories(){
        return _context.Categories.Where(c=>c.IsActive==true).OrderBy(c=>c.CreatedAt).ToList();
    }

    public List<Item> GetCategoryItems(int id,int pageNumber,int pageSize,out int totalRecords){
        var items=_context.Items.Where(i=>i.CategoryId==id && i.IsActive==true).OrderBy(i=>i.CreatedAt);
        totalRecords=items.Count();
        return items.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();
    }
    public List<Modifier> GetModifiers(int id,int pageNumber,int pageSize,out int totalRecords){
        if(id==0){
        var allmodifiers=_context.Modifiers.Where(i=> i.IsActive==true).OrderBy(i=>i.CreatedAt);
        totalRecords=allmodifiers.Count();
            return allmodifiers.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();
        }
        var modifiers=_context.Modifiers.Where(i=>i.ModifierGroupId==id && i.IsActive==true).OrderBy(i=>i.CreatedAt);
        totalRecords=modifiers.Count();
        return modifiers.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();
    }
   

    public bool AddCategory(Category category){
        var isCategory=_context.Categories.FirstOrDefault(c=>c.Name==category.Name && c.IsActive==true);
        if(isCategory!=null)return false;
        category.IsActive=true;
        _context.Categories.Add(category);
        _context.SaveChanges();
        return true;
    }
    public bool EditCategory(Category category){
        Category category1=_context.Categories.FirstOrDefault(c=>c.CategoryId==category.CategoryId);
        if(category1==null)return false;
        category1.IsActive=true;
        category1.Description=category.Description?? category1.Description;
        category1.Name=category.Name?? category1.Name;
        _context.Categories.Update(category1);
        _context.SaveChanges();
        return true;
    }

    public Category CategoryDetails(int id){
        return _context.Categories.Find(id);
    }

    public bool DeleteCategory(int id){
         var category=_context.Categories.FirstOrDefault(u=>u.CategoryId==id && u.IsActive==true);
            if(category==null)return false;
            category.IsActive=false;

            var items=_context.Items.Where(c=>c.CategoryId==id).ToList();
            foreach (var item in items)
            {
                item.IsActive=false;
            }

            var modifierGroups=_context.ModifierGroups.Where(mg=>mg.CategoryId==id && mg.IsActive==true).ToList();
            foreach (var modifierGroup in modifierGroups)
            {
                modifierGroup.IsActive=false;
            }
            _context.Categories.Update(category);
            _context.SaveChanges();
            return true;
    }

    public bool DeleteItem(int id){
        Item item=_context.Items.FirstOrDefault(i=>i.ItemId==id && i.IsActive==true);
        if(item==null)return false;
        item.IsActive=false;
        _context.Items.Update(item);
        _context.SaveChanges();
        return true;
    }
    public bool DeleteItems(JsonArray ids){
        if(ids.Count==0)return false;
        foreach(int itemId in ids){
            Item item=_context.Items.FirstOrDefault(i=>i.ItemId==itemId && i.IsActive==true);
            if(item==null)return false;
            item.IsActive=false;
        }
        _context.SaveChanges();
        return true;
    }

    public List<ModifierGroup> GetModifierGroups(){
        return _context.ModifierGroups.Where(m=>m.IsActive==true).ToList();
    }

    public bool AddItem(MenuItemsViewModel menuItemsViewModel){
        if(menuItemsViewModel.Name==null || menuItemsViewModel.CategoryId==null || menuItemsViewModel.ItemType==null || menuItemsViewModel.Rate==0 || menuItemsViewModel.Quantity==0 || menuItemsViewModel.Unit==null){
            return false;
        }
        Item item= new Item{
            Name=menuItemsViewModel.Name,
            CategoryId=menuItemsViewModel.CategoryId,
            ItemType=menuItemsViewModel.ItemType,
            Rate=menuItemsViewModel.Rate,
            Quantity=menuItemsViewModel.Quantity,
            Unit=menuItemsViewModel.Unit,
            Description=menuItemsViewModel.Description??null,
            DefaultTax=menuItemsViewModel.DefaultTax,
            IsAvailable=menuItemsViewModel.IsAvailable,
            TaxPercentage=menuItemsViewModel.TaxPercentage??null,
            ItemImg=menuItemsViewModel.ItemImg??null,
        };
        _context.Items.Add(item);
        _context.SaveChanges();
        return true;
    }

    public bool AddModifier(Modifier modifier){

         if(modifier.ModifierName==null || modifier.ModifierGroupId==null || modifier.Unit==null || modifier.Rate==0 || modifier.Quantity==0 ){
            return false;
        }     
        _context.Modifiers.Add(modifier);
        _context.SaveChanges();
        return true;
    }
    public bool DeleteModifier(int id){
        Modifier modifier=_context.Modifiers.FirstOrDefault(i=>i.ModifierId==id && i.IsActive==true);
        if(modifier==null)return false;
        modifier.IsActive=false;
        _context.Modifiers.Update(modifier);
        _context.SaveChanges();
        return true;
    }
      public bool DeleteModifiers(JsonArray ids){
        if(ids.Count==0)return false;
        foreach(int modifierId in ids){
            Modifier modifier=_context.Modifiers.FirstOrDefault(i=>i.ModifierId==modifierId && i.IsActive==true);
            if(modifier==null)return false;
            modifier.IsActive=false;
        }
        _context.SaveChanges();
        return true;
    }
      public Modifier ModifierDetails(int id){
        return _context.Modifiers.Find(id);
    }

      public bool EditModifier(int modifierId, string modifierGroupId,string modifierName,string unit,decimal rate,int quantity,string description){
        if(modifierName==null || modifierGroupId==null || unit==null || rate==0 || quantity==0 ){
            return false;
        }
        Modifier modifier1=_context.Modifiers.FirstOrDefault(c=>c.ModifierId==modifierId);
        modifier1.IsActive=true;
        modifier1.Description=description?? modifier1.Description;
        modifier1.ModifierName=modifierName?? modifier1.ModifierName;
        modifier1.Rate=rate==0? modifier1.Rate:rate;
        modifier1.Quantity=quantity==0? modifier1.Quantity:quantity;
        modifier1.Unit=unit?? modifier1.Unit;
        _context.Modifiers.Update(modifier1);
        _context.SaveChanges();
        return true;
    }

    public bool AddModifierGroup(JsonObject obj){
        string modifierGroupName=obj["modifierGroupName"].ToString();
        ModifierGroup modifierGroup2=_context.ModifierGroups.FirstOrDefault(mg=>mg.ModifierGroupName==modifierGroupName);
        if(obj["modifierGroupName"].ToString()==null || modifierGroup2!=null)return false;
        string Description=obj["Description"].ToString();
        ModifierGroup modifierGroup=new ModifierGroup{
            ModifierGroupName=modifierGroupName,
            Description=Description
        };
        _context.ModifierGroups.Add(modifierGroup);
        _context.SaveChanges();

        var modifierGroup1=_context.ModifierGroups.FirstOrDefault(mg=>mg.ModifierGroupName==modifierGroupName);
        JsonArray jsonArray=(JsonArray)obj["ids"];
        foreach (int i in jsonArray)
        {
            Modifier modifier1 = _context.Modifiers.FirstOrDefault(m => m.ModifierId == i);
            Modifier modifier=new Modifier{
                ModifierName=modifier1.ModifierName,
                Description=modifier1.Description,
                Unit=modifier1.Unit,
                Rate=modifier1.Rate,
                Quantity=modifier1.Quantity,
                ModifierGroupId=modifierGroup1.ModifierGroupId
            };
        _context.Modifiers.Add(modifier);
        }
        _context.SaveChanges();
    return true;
    }

}
