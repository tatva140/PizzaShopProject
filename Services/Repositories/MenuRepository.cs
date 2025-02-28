using DAL.Models;
using DAL.ViewModels;
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
        return _context.Categories.Where(c=>c.IsActive).ToList();
    }

    public List<Item> GetCategoryItems(int id){
        return _context.Items.Where(i=>i.CategoryId==id && i.IsActive==true).ToList();
    }

    public bool AddCategory(Category category){
        if(category.Name==null)return false;
        category.IsActive=true;
        _context.Categories.Add(category);
        _context.SaveChanges();
        return true;
    }
    public bool EditCategory(Category category){
        Console.Write(category.CategoryId);
        Category category1=_context.Categories.Find(category.CategoryId);
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
        Console.Write(id);
         var category=_context.Categories.FirstOrDefault(u=>u.CategoryId==id && u.IsActive==true);
            if(category==null)return false;
            category.IsActive=false;

            var items=_context.Items.Where(c=>c.CategoryId==id).ToList();
            foreach (var item in items)
            {
                item.IsActive=false;
            }
            _context.Categories.Update(category);
            _context.SaveChanges();
            return true;
    }
}
