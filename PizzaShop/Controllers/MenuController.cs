using System.Text.Json.Nodes;
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
    public IActionResult CategoryItems(int categoryId,int pageNumber=1,int pageSize=2,int selectedPage=2){
        var category=_menuService.GetCategories();
        var (items,totalRecords)=_menuService.GetCategoryItems(categoryId,pageNumber,selectedPage);
        var totalPage=(int)Math.Ceiling((double)totalRecords/selectedPage);
        Console.Write(totalPage);
         MenuItemsViewModel menuItemsViewModel = new MenuItemsViewModel{
            categories=category,
            items=items,
            PageNumber=pageNumber,
            PageSize=pageSize,
            TotalPages=totalPage,
            SelectedPage=selectedPage
        };
        ViewBag.categoryId=categoryId;
        return PartialView("_Items",menuItemsViewModel);
    }
[HttpPost]
    public IActionResult AddCategory(Category category){
        bool isAdded= _menuService.AddCategory(category);
        if(isAdded){
            TempData["Message"]="Category Added Successfully";
            TempData["MessageType"]="success";
        }else{
            TempData["Message"]="Category Already Exists";
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
            TempData["Message"]="Cannot Edit Category, it already Exists";
            TempData["MessageType"]="error";
        }
        return RedirectToAction("Index","Menu");
    }
    public IActionResult CategoryDetails(int id){
        var category=_menuService.CategoryDetails(id);
        
        return Json(new {name=category.Name, description=category.Description});
    }

[HttpPost]
    public IActionResult DeleteCategory([FromQuery] int id){
        bool deleted=_menuService.DeleteCategory(id);
        if(deleted)
        {
            TempData["Message"]="Category Deleted Successfully";
            TempData["MessageType"]="success";
        }
        else
        {
            TempData["Message"]="Category does not exist";
            TempData["MessageType"]="error";
        }
        return RedirectToAction("Index","Menu");
    }

[HttpGet]
    public IActionResult GetModifierGroups(int id){
        List<ModifierGroup> modifierGroup = _menuService.GetModifierGroups(id);
        return Json(modifierGroup);
    }

[HttpPost]
    public IActionResult DeleteItem(int itemId){
        bool isDeleted=_menuService.DeleteItem(itemId);
        if(isDeleted)
        {
            TempData["Message"]="Item Deleted Successfully";
            TempData["MessageType"]="success";
        }
        else
        {
            TempData["Message"]="Item cannot be deleted";
            TempData["MessageType"]="error";
        }
        return Ok(new {message="Deleted"});
    }

    [HttpPost]
    public IActionResult DeleteItems([FromBody] JsonArray ids){ 
        bool isDeleted=_menuService.DeleteItems(ids);
        if(isDeleted)
        {
            TempData["Message"]="All Items Deleted Successfully";
            TempData["MessageType"]="success";
        }
        else
        {
            TempData["Message"]="Items cannot be deleted";
            TempData["MessageType"]="error";
        }
         return Ok(new {redirectUrl=@Url.Action("Index","Menu")});
    }
}
