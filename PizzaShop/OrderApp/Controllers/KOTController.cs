using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Nodes;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol;
using Services.Service;
using static DAL.ViewModels.OrderAppKOTViewModel;

namespace PizzaShop.OrderApp.Controllers;

[Authorize(Roles="Chef,Account Manager")]
public class KOTController : Controller
{
    private readonly KOTService _kotService;
    private readonly MenuService _menuService;
    public KOTController(KOTService kotService, MenuService menuService)

    {
        _kotService = kotService;
        _menuService = menuService;

    }

    public IActionResult Index()
    {
        ViewData["Icon"] = "false";
        List<Category> categories = _menuService.GetCategories();
        OrderAppKOTViewModel orderAppKOTViewModel = new OrderAppKOTViewModel
        {
            categories = categories
        };
        return View("~/OrderApp/Views/KOT/Index.cshtml", orderAppKOTViewModel);
    }

    public IActionResult GetOrders(int categoryId, string categoryName, string status)
    {
        ViewData["Icon"] = "false";
        OrderAppKOTViewModel orderAppKOTViewModels = _kotService.GetOrders(categoryId, status);
        orderAppKOTViewModels.CategoryName = categoryName;
        orderAppKOTViewModels.CategoryId = categoryId;
        return View("~/OrderApp/Views/Shared/_KOTOrders.cshtml", orderAppKOTViewModels);
    }

    [HttpPost]
    public IActionResult GetOrderItems([FromBody] JsonArray orderItems)
    {
        ViewData["Icon"] = "false";
        string order = orderItems.ToJsonString();
        List<KOTItemListViewModel> orderAppKOTViewModels = JsonConvert.DeserializeObject<List<KOTItemListViewModel>>(order);
        return View("~/OrderApp/Views/Shared/_KOTItemsMarkAsPrepared.cshtml", orderAppKOTViewModels);
    }

    [HttpPost]
    public IActionResult MarkAsPrepared([FromBody] JsonArray orderItems)
    {
        string order = orderItems.ToJsonString();
        List<MarkAsPrepared> orderAppKOTViewModels = JsonConvert.DeserializeObject<List<MarkAsPrepared>>(order);
        _kotService.MarkAsPrepared(orderAppKOTViewModels);
        return Json(new { success = true });
    }
}
