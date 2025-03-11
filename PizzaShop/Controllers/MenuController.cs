using System.Text.Json.Nodes;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Service;

namespace PizzaShop.Controllers;

public class MenuController : Controller
{
    private readonly MenuService _menuService;

    public MenuController(MenuService menuService)
    {
        _menuService = menuService;
    }
    public ActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult MenuItems(int categoryId)
    {
        List<Category> category = _menuService.GetCategories();
        MenuItemsViewModel menuItemsViewModel = new MenuItemsViewModel
        {
            categories = category,
            ShowList = categoryId
        };
        return PartialView("_MenuItems", menuItemsViewModel);
    }

    [HttpGet]
    public IActionResult MenuModifiers(int modifierId, int pageNumber = 1, int pageSize = 2, int selectedPage = 2)
    {
        List<ModifierGroup> modifierGroup = _menuService.GetModifierGroups();
        var (modifiers, totalRecords) = _menuService.GetModifiers(modifierId, pageNumber, selectedPage);
        int totalPage = (int)Math.Ceiling((double)totalRecords / selectedPage);
        MenuModifiersViewModel mennuModifiersViewModel = new MenuModifiersViewModel
        {
            modifierGroups = modifierGroup,
            modifier = modifiers,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPage,
            SelectedPage = selectedPage,
            ShowList = modifierId
        };
        return PartialView("_ModifierGroups", mennuModifiersViewModel);
    }

    [HttpGet]
    public IActionResult CategoryItems(int categoryId, int pageNumber = 1, int pageSize = 2, int selectedPage = 2)
    {
        List<Category> category = _menuService.GetCategories();
        var (items, totalRecords) = _menuService.GetCategoryItems(categoryId, pageNumber, selectedPage);
        int totalPage = (int)Math.Ceiling((double)totalRecords / selectedPage);
        MenuItemsViewModel menuItemsViewModel = new MenuItemsViewModel
        {
            categories = category,
            items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPage,
            SelectedPage = selectedPage
        };
        ViewBag.categoryId = categoryId;
        return PartialView("_Items", menuItemsViewModel);
    }
    [HttpGet]
    public IActionResult Modifiers(int modifierId, int pageNumber = 1, int pageSize = 2, int selectedPage = 2)
    {
        List<ModifierGroup> modifierGroups = _menuService.GetModifierGroups();
        var (modifiers, totalRecords) = _menuService.GetModifiers(modifierId, pageNumber, selectedPage);
        int totalPage = (int)Math.Ceiling((double)totalRecords / selectedPage);
        MenuModifiersViewModel mennuModifiersViewModel = new MenuModifiersViewModel
        {
            modifierGroups = modifierGroups,
            modifier = modifiers,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPage,
            SelectedPage = selectedPage
        };
        return PartialView("_Modifiers", mennuModifiersViewModel);
    }
    [HttpGet]
    public IActionResult ModifierTable(int modifierId, int pageNumber = 1, int pageSize = 2, int selectedPage = 2)
    {
        List<ModifierGroup> modifierGroups = _menuService.GetModifierGroups();
        var (modifiers, totalRecords) = _menuService.GetModifiers(modifierId, pageNumber, selectedPage);
        int totalPage = (int)Math.Ceiling((double)totalRecords / selectedPage);
        MenuModifiersViewModel mennuModifiersViewModel = new MenuModifiersViewModel
        {
            modifierGroups = modifierGroups,
            modifier = modifiers,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPage,
            SelectedPage = selectedPage
        };
        return PartialView("_modifierTable", mennuModifiersViewModel);
    }

    [HttpPost]
    public IActionResult AddCategory([FromBody] JsonObject obj)
    {
        string data = obj.ToJsonString();
        Category category = JsonConvert.DeserializeObject<Category>(data);
        int isAdded = _menuService.AddCategory(category);
        if (isAdded != 0)
        {
            TempData["Message"] = "Category Added Successfully";
            TempData["MessageType"] = "success";
        }
        else
        {
            TempData["Message"] = "Category Already Exists";
            TempData["MessageType"] = "error";
        }
        return RedirectToAction("MenuItems", new { categoryId = isAdded });

    }
    [HttpPost]
    public IActionResult AddModifier(Modifier modifier)
    {
        int isAdded = _menuService.AddModifier(modifier);
        if (isAdded != 0)
        {
            TempData["Message"] = "Modifier Added Successfully";
            TempData["MessageType"] = "success";
        }
        else
        {
            TempData["Message"] = "Modifier Cannot be added";
            TempData["MessageType"] = "error";
        }
        return RedirectToAction("Modifiers", new { modifierId = isAdded });

    }

    [HttpPost]
    public IActionResult EditCategory([FromBody] JsonObject obj)
    {
        string data = obj.ToJsonString();
        Category category = JsonConvert.DeserializeObject<Category>(data);
        bool isEdited = _menuService.EditCategory(category);
        if (isEdited)
        {
            TempData["Message"] = "Category Edited Successfully";
            TempData["MessageType"] = "success";
        }
        else
        {
            TempData["Message"] = "Cannot Edit Category, it already Exists";
            TempData["MessageType"] = "error";
        }
        return RedirectToAction("MenuItems");

    }
    public IActionResult CategoryDetails(int id)
    {
        Category category = _menuService.CategoryDetails(id);
        return Json(new { name = category.Name, description = category.Description });
    }

    [HttpPost]
    public IActionResult DeleteCategory([FromBody] int id)
    {
        bool deleted = _menuService.DeleteCategory(id);
        if (deleted)
        {
            TempData["Message"] = "Category Deleted Successfully";
            TempData["MessageType"] = "success";
        }
        else
        {
            TempData["Message"] = "Category does not exist";
            TempData["MessageType"] = "error";
        }
        return RedirectToAction("MenuItems", "Menu");
    }

    [HttpGet]
    public IActionResult GetModifierGroups()
    {
        List<ModifierGroup> modifierGroup = _menuService.GetModifierGroups();
        return Json(modifierGroup);
    }
    [HttpGet]
    public IActionResult GetModifiers(int modifierId, int pageNumber = 1, int pageSize = 2, int selectedPage = 2)
    {
        var (modifiers, totalRecords) = _menuService.GetModifiers(modifierId, pageNumber, selectedPage);
        return Json(modifiers);
    }

    [HttpPost]
    public IActionResult DeleteItem(int itemId)
    {
        bool isDeleted = _menuService.DeleteItem(itemId);
        if (isDeleted)
        {
            TempData["Message"] = "Item Deleted Successfully";
            TempData["MessageType"] = "success";
        }
        else
        {
            TempData["Message"] = "Item cannot be deleted";
            TempData["MessageType"] = "error";
        }
        return Ok(new { message = "Deleted" });
    }

    [HttpPost]
    public IActionResult DeleteItems([FromBody] JsonArray ids)
    {
        bool isDeleted = _menuService.DeleteItems(ids);
        if (isDeleted)
        {
            TempData["Message"] = "All Items Deleted Successfully";
            TempData["MessageType"] = "success";
        }
        else
        {
            TempData["Message"] = "Items cannot be deleted";
            TempData["MessageType"] = "error";
        }
        return Ok(new { redirectUrl = @Url.Action("Index", "Menu") });
    }
    [HttpPost]
    public IActionResult DeleteModifiers([FromBody] JsonArray ids)
    {
        bool isDeleted = _menuService.DeleteModifiers(ids);
        if (isDeleted)
        {
            TempData["Message"] = "Selected Modifiers Deleted Successfully";
            TempData["MessageType"] = "success";
        }
        else
        {
            TempData["Message"] = "Modifiers cannot be deleted";
            TempData["MessageType"] = "error";
        }
        return Ok(new { redirectUrl = @Url.Action("Index", "Menu") });
    }

    [HttpPost]
    public IActionResult AddItem([FromBody] JsonObject obj)
    {

        string data = obj.ToJsonString();
        MenuItemsViewModel menuItemsViewModel = JsonConvert.DeserializeObject<MenuItemsViewModel>(data);
        int isAdded = _menuService.AddItem(menuItemsViewModel);
        if (isAdded!=0)
        {
            TempData["success"] = "Items Added Successfully";

        }
        else
        {
            TempData["Message"] = "Items cannot be added";
            TempData["MessageType"] = "error";
        }
        return RedirectToAction("CategoryItems", new { categoryId = isAdded });


    }


    [HttpPost]
    public IActionResult DeleteModifier(int id)
    {
        bool isDeleted = _menuService.DeleteModifier(id);
        if (isDeleted)
        {
            TempData["Message"] = "Modifier Deleted Successfully";
            TempData["MessageType"] = "success";
        }
        else
        {
            TempData["Message"] = "Modifier cannot be deleted";
            TempData["MessageType"] = "error";
        }
        return Ok(new { message = "Deleted" });
    }

    [HttpGet]
    public IActionResult ModifierDetails(int id)
    {
        Modifier modifier = _menuService.ModifierDetails(id);
        return Json(new { name = modifier.ModifierName, description = modifier.Description, rate = modifier.Rate, quantity = modifier.Quantity, unit = modifier.Unit });
    }
    [HttpPost]
    public IActionResult EditModifier(Modifier modifier)
    {

        int isEdited = _menuService.EditModifier(modifier);
        if (isEdited != 0)
        {
            TempData["Message"] = "Modifier Edited Successfully";
            TempData["MessageType"] = "success";
        }
        else
        {
            TempData["Message"] = "Cannot Edit Modifier, it already Exists";
            TempData["MessageType"] = "error";
        }
        return RedirectToAction("Modifiers", new { modifierId = isEdited });


    }

    [HttpPost]
    public IActionResult AddModifierGroup([FromBody] JsonObject obj)
    {
        int isAdded = _menuService.AddModifierGroup(obj);
        if (isAdded != 0)
        {
            TempData["Message"] = "Modifier Group Added Successfully";
            TempData["MessageType"] = "success";
        }
        else
        {
            TempData["Message"] = "Modifier Group Cannot be added";
            TempData["MessageType"] = "error";
        }
        return RedirectToAction("MenuModifiers", new { modifierId = isAdded });

    }
    [HttpPost]
    public IActionResult EditModifierGroup([FromBody] JsonObject obj)
    {
        string data = obj.ToJsonString();
        MenuModifiersViewModel menuModifiersViewModel = JsonConvert.DeserializeObject<MenuModifiersViewModel>(data);
        int isEdited = _menuService.EditModifierGroup(menuModifiersViewModel);
        if (isEdited != 0)
        {
            TempData["Message"] = "Modifier Group Added Successfully";
            TempData["MessageType"] = "success";
        }
        else
        {
            TempData["Message"] = "Modifier Group Cannot be added";
            TempData["MessageType"] = "error";
        }
        return Ok(new { id = isEdited });

    }

    [HttpPost]
    public IActionResult DeleteModifierGroup([FromBody] int id)
    {
        bool isDeleted = _menuService.DeleteModifierGroup(id);
        if (isDeleted)
        {
            TempData["Message"] = "Modifier Group Added Successfully";
            TempData["MessageType"] = "success";
        }
        else
        {
            TempData["Message"] = "Modifier Group Cannot be added";
            TempData["MessageType"] = "error";
        }
        return RedirectToAction("MenuModifiers", "Menu");
    }


    [HttpGet]
    public IActionResult GetModifierGroupDetails(int id)
    {
        ModifierGroup modifierGroup = _menuService.GetModifierGroupDetails(id);
        return Ok(new { modifierGroup = modifierGroup });
    }

    [HttpGet]
    public IActionResult FetchModifier(int id)
    {
        List<Modifier> modifiers = _menuService.GetAllModifiers(id);
        return Json(new { modifier = modifiers });
    }
}
