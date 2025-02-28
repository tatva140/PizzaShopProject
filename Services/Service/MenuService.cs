using DAL.Models;
using DAL.ViewModels;
using Services.Interfaces;
using Services.Repositories;

namespace Services.Service;

public class MenuService
{
    private readonly IMenuRepository _menuRepository;

    public MenuService(IMenuRepository menuRepository){
        _menuRepository=menuRepository;
    }
    public List<Category> GetCategories(){
       return _menuRepository.GetCategories();
    }

    public List<Item> GetCategoryItems(int id){
        return _menuRepository.GetCategoryItems(id);
    }

    public bool AddCategory(Category category){
        return _menuRepository.AddCategory(category);
    }
    public bool EditCategory(Category category){
        return _menuRepository.EditCategory(category);
    }

    public Category CategoryDetails(int id){
        return _menuRepository.CategoryDetails(id);
    }

    public bool DeleteCategory(int id){
        Console.Write("here");
        return _menuRepository.DeleteCategory(id);
    }
}
