using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Service;

namespace PizzaShop.OrderApp.Controllers;

[Authorize(Roles = "Account Manager")]

public class OrderMenuController : Controller
{
    private readonly OrderAppMenuService _orderAppMenuService;
    public OrderMenuController(OrderAppMenuService orderAppMenuService)
    {
        _orderAppMenuService = orderAppMenuService;
    }

    public IActionResult Index(int id)
    {
        ViewData["Icon"] = "false";
        if (id != 0)
        {
            ViewData["showModal"] = "true";
        }
        else
        {
            ViewData["showModal"] = "false";
        }
        OrderAppMenuViewModel orderAppMenuViewModel = _orderAppMenuService.GetCategories();
        return View("~/OrderApp/Views/OrderMenu/Index.cshtml", orderAppMenuViewModel);
    }

    public IActionResult GetItems(int id)
    {
        ViewData["Icon"] = "false";
        OrderAppMenuViewModel orderAppMenuViewModel = _orderAppMenuService.GetItems(id);
        return View("~/OrderApp/Views/Shared/_ItemCard.cshtml", orderAppMenuViewModel);

    }

    public IActionResult FavouriteItem(int id, bool flag)
    {
        ViewData["Icon"] = "false";
        _orderAppMenuService.FavouriteItem(id, flag);
        return Ok(new { message = "Favourite" });
    }

    public IActionResult OrderBrief()
    {
        ViewData["Icon"] = "false";

        return View("~/OrderApp/Views/Shared/_OrderPreview.cshtml");

    }
}
