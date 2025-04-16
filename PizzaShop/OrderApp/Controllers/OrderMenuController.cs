using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Nodes;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        ViewData["id"]=id;
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

    public IActionResult OrderBrief(int id)
    {
        ViewData["Icon"] = "false";
        OrderAppMenuViewModel orderAppMenuViewModel = _orderAppMenuService.GetOrderPreview(id);
        return View("~/OrderApp/Views/Shared/_OrderPreview.cshtml",orderAppMenuViewModel);
    }
    public IActionResult GetModifierDetails(int id)
    {
        ViewData["Icon"] = "false";
        OrderAppMenuViewModel orderAppMenuViewModel = _orderAppMenuService.GetModifierDetails(id);
        return View("~/OrderApp/Views/Shared/_ModifiersCardList.cshtml",orderAppMenuViewModel);
    }

    [HttpPost]
    public IActionResult RenderOrderItem([FromBody] JsonObject orderItems)
    {
        ViewData["Icon"] = "false";
        string data = orderItems.ToJsonString();
        OrdersListViewModel ordersListViewModel = JsonConvert.DeserializeObject<OrdersListViewModel>(data);
        
       OrderAppMenuViewModel orderAppMenuViewModel =new OrderAppMenuViewModel{
        ordersListViewModel=ordersListViewModel
       };
        return View("~/OrderApp/Views/Shared/_OrderPrevItems.cshtml", orderAppMenuViewModel);
    }

    [HttpPost]
    public IActionResult RenderNewOrderItem([FromBody] JsonArray orderItems)
    {
        ViewData["Icon"] = "false";
        string data = orderItems.ToJsonString();
        List<OrderItemListViewModel> orderItemListViewModels = JsonConvert.DeserializeObject<List<OrderItemListViewModel>>(data);

       OrderAppMenuViewModel orderAppMenuViewModel =new OrderAppMenuViewModel{
        ordersListViewModel=new OrdersListViewModel{
            itemLists=orderItemListViewModels
        }
       };
        return View("~/OrderApp/Views/Shared/_OrderPrevItems.cshtml", orderAppMenuViewModel);
    }
    // [HttpPost]
    // public IActionResult AddOrderItem(int id,string modifierList){
    //      List<int> selectedModifiers = JsonConvert.DeserializeObject<List<int>>(modifierList);
    //      OrderAppMenuViewModel orderAppMenuViewModel=new OrderAppMenuViewModel{
    //         modifierList=selectedModifiers,
    //         id=id
    //      };
    //      _orderAppMenuService.AddOrderItem(orderAppMenuViewModel);
    //      return Ok();
    // }
}
