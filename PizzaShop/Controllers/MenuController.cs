using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Service;

namespace PizzaShop.Controllers;

public class MenuController:Controller
{
    private readonly MenuService _menuService;

    public MenuController(MenuService menuService){
        _menuService=menuService;
    }
    public ActionResult Index()
    {
        return View();
    }

[HttpGet]
    public IActionResult MenuItems()
    {
        var category=_menuService.GetCategories();
        MenuItemsViewModel menuItemsViewModel = new MenuItemsViewModel{
            categories=category,
        };
        return PartialView("_MenuItems",menuItemsViewModel);
    }

[HttpGet]
    public IActionResult CategoryItems(int id){
    
        var items=_menuService.GetCategoryItems(id);
        return PartialView("_Items",items);
    }

[HttpPost]
    public IActionResult AddCategory(Category category){
        Console.WriteLine(category.Name);
        bool isAdded= _menuService.AddCategory(category);
        if(isAdded){
            TempData["Message"]="Category Added Successfully";
            TempData["MessageType"]="success";
        }else{
            TempData["Message"]="Cannot Add Category";
            TempData["MessageType"]="error";
        }
        return RedirectToAction("Index","Menu");
    }

    [HttpPost]
     public IActionResult EditCategory([FromQuery] int id,Category category){
        category.CategoryId=id;
        bool isEdited= _menuService.EditCategory(category);
        if(isEdited){
            TempData["Message"]="Category Edited Successfully";
            TempData["MessageType"]="success";
        }else{
            TempData["Message"]="Cannot Edit Category";
            TempData["MessageType"]="error";
        }
        return RedirectToAction("Index","Menu");
    }
    public IActionResult CategoryDetails(int id){
        var category=_menuService.CategoryDetails(id);
        
        return Json(new {name=category.Name, description=category.Description});
    }

    public IActionResult DeleteCategory([FromQuery] int id){
        Console.Write(id);
        bool deleted=_menuService.DeleteCategory(id);
        if(deleted)
        {
            TempData["Message"]="User Deleted Successfully";
            TempData["MessageType"]="success";
        }
        else
        {
            TempData["Message"]="User does not exist";
            TempData["MessageType"]="error";
        }
        return RedirectToAction("Index","Menu");
    }
}
