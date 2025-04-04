using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Service;

namespace PizzaShop.OrderApp.Controllers;

public class KOTController : Controller
{
    private readonly KOTService _kotService;
    private readonly MenuService _menuService;
    public KOTController(KOTService kotService,MenuService menuService)
    
    {
        _kotService = kotService;
        _menuService = menuService;

    }
    
    public IActionResult Index()
    {
        ViewData["Icon"] = "false";
        List<Category> categories = _menuService.GetCategories();
        OrderAppKOTViewModel orderAppKOTViewModel = new OrderAppKOTViewModel{
            categories = categories
        };
        return View("~/OrderApp/Views/KOT/Index.cshtml",orderAppKOTViewModel);
    }

    // public IActionResult GetOrders(int categoryId)
    // {
    //     ViewData["Icon"] = "false";
    //     List<OrderAppKOTViewModel> orderAppKOTViewModels = _kotService.GetOrders(categoryId);
    //     return View("~/OrderApp/Views/Shared/KOTOrders.cshtml");
    // }
}
