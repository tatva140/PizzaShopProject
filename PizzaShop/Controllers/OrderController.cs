using Microsoft.AspNetCore.Mvc;
using Services.Service;

namespace PizzaShop.Controllers;

public class OrderController:Controller
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
    public IActionResult Orders(string search,string status,string time,string from,string to,int pageNumber = 1, int selectedPage = 2)
    {
        var (orders, totalRecords) = _order.GetOrders(search,status,time,from,to, pageNumber, selectedPage);
        int totalPage = (int)Math.Ceiling((double)totalRecords / selectedPage);
      
        ViewBag.orderSearch = search;
        ViewBag.orderStatus = status;
        ViewBag.orderTime = time;
        return PartialView("_Order", orders);
    }
     [HttpPost]
    public IActionResult ExcelUpload(string search,string status,string time,string from,string to)
    {
        var isUploaded= _order.UploadExcel(search,status,time,from,to);
       
            TempData["success"] = "Excel Uploaded Successfully";
     
             
        ViewBag.orderSearch = search;
        ViewBag.orderStatus = status;
        ViewBag.orderTime = time;
        return Ok(new{message="Excel Uploaded Successfully"});
    }

}
