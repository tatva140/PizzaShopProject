using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SelectPdf;
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
    
    
    public IActionResult OrderPdf( int id){
        ViewData["selected"]="Orders";
        OrdersListViewModel model=_order.GetOrderDetails(id);
        var htmlContent = RenderViewToString("OrderPdf", model);
        HtmlToPdf converter = new HtmlToPdf();
        PdfDocument document = converter.ConvertHtmlString(htmlContent);
        var stream = new MemoryStream();
        document.Save(stream);
        document.Close();
        return File(stream.ToArray(), "application/pdf", "OrderDetails.pdf");
    }
    
    private string RenderViewToString(string viewName, object model)
    {
        ViewData.Model = model;

        using (var writer = new StringWriter())
        {
            var viewResult = HttpContext.RequestServices
                .GetService(typeof(Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine)) 
                as Microsoft.AspNetCore.Mvc.ViewEngines.ICompositeViewEngine;

            var view = viewResult.FindView(ControllerContext, viewName, false).View;
            var viewContext = new ViewContext(
                ControllerContext,
                view,
                ViewData,
                TempData,
                writer,
                new Microsoft.AspNetCore.Mvc.ViewFeatures.HtmlHelperOptions()
            );

            view.RenderAsync(viewContext).Wait();
            return writer.GetStringBuilder().ToString();
        }
    }
    
}
