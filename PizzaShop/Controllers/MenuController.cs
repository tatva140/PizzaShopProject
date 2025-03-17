using System.Text.Json.Nodes;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Service;
using Services.Utilities;

namespace PizzaShop.Controllers;

public class MenuController : Controller
{
    private readonly MenuService _menuService;
    private readonly FileUploads _fileUploads;


    public MenuController(MenuService menuService,FileUploads fileUploads)
    {
        _menuService = menuService;
        _fileUploads=fileUploads;
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
    public IActionResult MenuModifiers(int modifierId,string search, int pageNumber = 1, int pageSize = 2, int selectedPage = 2)
    {
        List<ModifierGroup> modifierGroup = _menuService.GetModifierGroups();
        var (modifiers, totalRecords) = _menuService.GetModifiers(modifierId,search, pageNumber, selectedPage);
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
        ViewBag.ModifierSearch=search;
        return PartialView("_ModifierGroups", mennuModifiersViewModel);
    }

    [HttpGet]
    public IActionResult CategoryItems(int categoryId,string search, int pageNumber = 1, int pageSize = 2, int selectedPage = 2)
    {
        List<Category> category = _menuService.GetCategories();
        var (items, totalRecords) = _menuService.GetCategoryItems(categoryId,search, pageNumber, selectedPage);
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
        ViewBag.ItemSearch = search;
        return PartialView("_Items", menuItemsViewModel);
    }
    [HttpGet]
    public IActionResult Modifiers(int modifierId,string search, int pageNumber = 1, int pageSize = 2, int selectedPage = 2)
    {
        List<ModifierGroup> modifierGroups = _menuService.GetModifierGroups();
        var (modifiers, totalRecords) = _menuService.GetModifiers(modifierId,search, pageNumber, selectedPage);
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
        ViewBag.ModifierSearch=search;

        return PartialView("_Modifiers", mennuModifiersViewModel);
    }
    [HttpGet]
    public IActionResult ModifierTable(int modifierId,string search, int pageNumber = 1, int pageSize = 2, int selectedPage = 2)
    {
        List<ModifierGroup> modifierGroups = _menuService.GetModifierGroups();
        var (modifiers, totalRecords) = _menuService.GetModifiers(modifierId,search, pageNumber, selectedPage);
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
        ViewBag.ModifierSearch=search;

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
            TempData["success"] = "Category Added Successfully";
        }
        else
        {
            TempData["error"] = "Category Already Exists";
        }
        return RedirectToAction("MenuItems", new { categoryId = isAdded });

    }
    [HttpPost]
    public IActionResult AddModifier(Modifier modifier)
    {
        int isAdded = _menuService.AddModifier(modifier);
        if (isAdded != 0)
        {
            TempData["success"] = "Modifier Added Successfully";
        }
        else
        {
            TempData["error"] = "Modifier Cannot be added";
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
            TempData["success"] = "Category Edited Successfully";
        }
        else
        {
            TempData["error"] = "Cannot Edit Category, it already Exists";
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
            TempData["success"] = "Category Deleted Successfully";
        }
        else
        {
            TempData["error"] = "Category does not exist";
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
    public IActionResult GetModifiers(int modifierId,string search, int pageNumber = 1, int pageSize = 2, int selectedPage = 2)
    {
        var (modifiers, totalRecords) = _menuService.GetModifiers(modifierId,search, pageNumber, selectedPage);
        ViewBag.ModifierSearch=search;

        return Json(modifiers);
    }

    [HttpPost]
    public IActionResult DeleteItem(int itemId)
    {
        bool isDeleted = _menuService.DeleteItem(itemId);
        if (isDeleted)
        {
            TempData["success"] = "Item Deleted Successfully";
        }
        else
        {
            TempData["error"] = "Item cannot be deleted";
        }
        return Ok(new { message = "Deleted" });
    }

    [HttpPost]
    public IActionResult DeleteItems([FromBody] JsonArray ids)
    {
        bool isDeleted = _menuService.DeleteItems(ids);
        if (isDeleted)
        {
            TempData["success"] = "All Items Deleted Successfully";
        }
        else
        {
            TempData["error"] = "Items cannot be deleted";
        }
        return Ok(new { redirectUrl = @Url.Action("Index", "Menu") });
    }
    [HttpPost]
    public IActionResult DeleteModifiers([FromBody] JsonArray ids)
    {
        bool isDeleted = _menuService.DeleteModifiers(ids);
        if (isDeleted)
        {
            TempData["success"] = "Selected Modifiers Deleted Successfully";
        }
        else
        {
            TempData["error"] = "Modifiers cannot be deleted";
        }
        return Ok(new { redirectUrl = @Url.Action("Index", "Menu") });
    }

    [HttpPost]
    public IActionResult AddItem(MenuItemsViewModel menuItemsViewModel,IFormFile ItemImg)
    {
        if (ItemImg != null)
        {
            if (ItemImg.Length > 0)
            {
                string fileName=_fileUploads.UploadProfileImage(ItemImg);
                menuItemsViewModel.ItemImg="/images/"+fileName;

            }
        }
        int isAdded = _menuService.AddItem(menuItemsViewModel);
        if (isAdded!=0)
        {
            TempData["success"] = "Item Added Successfully";

        }
        else
        {
            TempData["error"] = "Item cannot be added";
        }
        return RedirectToAction("CategoryItems", new { categoryId = isAdded });


    }
    [HttpPost]
    public IActionResult EditItem(MenuItemsViewModel menuItemsViewModel,IFormFile ItemImg)
    {

       if (ItemImg != null)
        {
            if (ItemImg.Length > 0)
            {
                string fileName=_fileUploads.UploadProfileImage(ItemImg);
                menuItemsViewModel.ItemImg="/images/"+fileName;
            }
        }
        int isEdited = _menuService.EditItem(menuItemsViewModel);
        if (isEdited!=0)
        {
            TempData["success"] = "Item Edited Successfully";

        }
        else
        {
            TempData["error"] = "Item cannot be added";
        }
        return RedirectToAction("CategoryItems", new { categoryId = isEdited });


    }


    [HttpPost]
    public IActionResult DeleteModifier(int id)
    {
        bool isDeleted = _menuService.DeleteModifier(id);
        if (isDeleted)
        {
            TempData["success"] = "Modifier Deleted Successfully";
        }
        else
        {
            TempData["error"] = "Modifier cannot be deleted";
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
            TempData["success"] = "Modifier Edited Successfully";
        }
        else
        {
            TempData["error"] = "Cannot Edit Modifier, it already Exists";
        }
        return RedirectToAction("Modifiers", new { modifierId = isEdited });


    }

    [HttpPost]
    public IActionResult AddModifierGroup([FromBody] JsonObject obj)
    {
        int isAdded = _menuService.AddModifierGroup(obj);
        if (isAdded != 0)
        {
            TempData["success"] = "Modifier Group Added Successfully";
        }
        else
        {
            TempData["error"] = "Modifier Group Cannot be added";
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
            TempData["success"] = "Modifier Group Edited Successfully";
        }
        else
        {
            TempData["error"] = "Modifier Group Cannot be edited";
        }
        return Ok(new { id = isEdited });

    }

    [HttpPost]
    public IActionResult DeleteModifierGroup([FromBody] int id)
    {
        bool isDeleted = _menuService.DeleteModifierGroup(id);
        if (isDeleted)
        {
            TempData["success"] = "Modifier Group Added Successfully";
        }
        else
        {
            TempData["error"] = "Modifier Group Cannot be added";
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
    [HttpGet]
    public IActionResult FetchItemDetails(int id){
        EditItemViewModel  editItemViewModel=_menuService.FetchItemDetails(id);
        return PartialView("_EditItemModal",editItemViewModel);
    }
    public IActionResult GetMGDetails(int id){
        List<ModifierGroup> modifierGroups=_menuService.GetMGDetails(id);
         return Json(new { modifierGroups = modifierGroups });
    }
    public IActionResult GetMinMax(int id,int itemId){
        List<int> minMaxValues=_menuService.GetMinMax(id,itemId);
         return Json(new { minMaxValues = minMaxValues });

    }
    public IActionResult GetModifiersForItemEdit(int modifierId){
        List<Modifier> modifiers=_menuService.GetModifiersForItemEdit(modifierId);
        
                return Json(modifiers);

    }
}
