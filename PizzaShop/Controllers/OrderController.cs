using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Service;

namespace PizzaShop.Controllers;

public class OrderController : Controller
{
    private readonly OrderService _order;
    public OrderController(OrderService order)
    {
        _order = order;
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Orders(string search, string status, string time, string from, string to, int pageNumber = 1, int selectedPage = 2)
    {
        var (orders, totalRecords) = _order.GetOrders(search, status, time, from, to, pageNumber, selectedPage);
        int totalPage = (int)Math.Ceiling((double)totalRecords / selectedPage);

    OrdersViewModel model = new OrdersViewModel
    {
        Orders = orders,
        TotalRecords = totalRecords,
        TotalPages = totalPage,
        SelectedPage = selectedPage,
        PageNumber = pageNumber
    };
        ViewBag.orderSearch = search;
        ViewBag.orderStatus = status;
        ViewBag.orderTime = time;
        return PartialView("_Order", model);
    }
    [HttpPost]
    public IActionResult ExcelUpload(string search, string status, string time, string from, string to)
    {
        FileContentResult isUploaded = _order.UploadExcel(search, status, time, from, to);
        TempData["success"] = "Excel Uploaded Successfully";
        ViewBag.orderSearch = search;
        ViewBag.orderStatus = status;
        ViewBag.orderTime = time;
        Response.Headers["Content-Disposition"] = $"attachment; filename=\"Orders.xlsx\"";
        return isUploaded;
    }

    [HttpGet]
    public IActionResult OrderDetails([FromQuery] int id)
    {
        OrdersListViewModel ordersListViewModel =_order.GetOrderDetails(id);
        return View(ordersListViewModel);
    }
}
