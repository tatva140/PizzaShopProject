using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Service;

namespace PizzaShop.OrderApp.Controllers;

public class OrderMenuController : Controller
{
    private readonly OrderAppMenuService _orderAppMenuService;
    public OrderMenuController(OrderAppMenuService orderAppMenuService)
    {
        _orderAppMenuService = orderAppMenuService;
    }
    public IActionResult Index()
    {
        ViewData["Icon"] = "false";
        OrderAppMenuViewModel orderAppMenuViewModel = _orderAppMenuService.GetCategories();
        return View("~/OrderApp/Views/OrderMenu/Index.cshtml", orderAppMenuViewModel);
    }

    public IActionResult GetItems(int id)
    {
        ViewData["Icon"] = "false";
        OrderAppMenuViewModel orderAppMenuViewModel = _orderAppMenuService.GetItems(id);
        return View("~/OrderApp/Views/Shared/_ItemCard.cshtml", orderAppMenuViewModel);

    }

    public IActionResult FavouriteItem(int id,bool flag){
        ViewData["Icon"] = "false";
        _orderAppMenuService.FavouriteItem(id,flag);
        return Ok(new {message="Favourite"});
    }
}
