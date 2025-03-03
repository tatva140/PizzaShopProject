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
    public IActionResult CategoryItems(int id,int pageNumber=1,int pageSize=2,int selectedPage=2){
        var category=_menuService.GetCategories();
        var (items,totalRecords)=_menuService.GetCategoryItems(id,pageNumber,selectedPage);
        var totalPage=(int)Math.Ceiling((double)totalRecords/selectedPage);
         MenuItemsViewModel menuItemsViewModel = new MenuItemsViewModel{
            categories=category,
            items=items,
            PageNumber=pageNumber,
            PageSize=pageSize,
            TotalPages=totalPage,
            SelectedPage=selectedPage
        };
        return PartialView("_Items",menuItemsViewModel);
    }
[HttpPost]
    public IActionResult AddCategory(Category category){
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
     public IActionResult EditCategory(int id,Category category){
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

[HttpGet]
    public IActionResult GetModifierGroups(int id){
        List<ModifierGroup> modifierGroup = _menuService.GetModifierGroups(id);
        return Json(modifierGroup);
    }
}
