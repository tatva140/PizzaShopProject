using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Service;

namespace PizzaShop.Controllers;

[Authorize]
public class CustomerController : Controller
{
    private readonly CustomerService _customerService;
    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }
    [PermissionsAtrribute("Customers", "CanAddEdit")]
    public ActionResult AddEdit()
    {
        return Ok();
    }
    [PermissionsAtrribute("Customers", "CanDelete")]
    public ActionResult Delete()
    {
        return Ok();
    }
    [PermissionsAtrribute("Customers", "CanView")]
    public ActionResult ViewPermission()
    {
        return Ok();
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Customers(string search, string sortOrder, string time, string from, string to, int pageNumber = 1, int selectedPage = 2)
    {
        var (customers, totalRecords) = _customerService.GetCustomers(search, sortOrder, time, from, to, pageNumber, selectedPage);
        int totalPage = (int)Math.Ceiling((double)totalRecords / selectedPage);

        CustomersViewModel model = new CustomersViewModel
        {
            Customers = customers,
            TotalPages = totalPage,
            SelectedPage = selectedPage,
            PageNumber = pageNumber
        };
        ViewBag.customerSearch = search;

        return PartialView("_Customer", model);
    }
    [HttpPost]
    public IActionResult ExcelUpload(string search, string time, string from, string to)
    {
        FileContentResult isUploaded = _customerService.UploadExcel(search, time, from, to);
        TempData["success"] = "Excel Uploaded Successfully";
        ViewBag.orderSearch = search;
        Response.Headers["Content-Disposition"] = $"attachment; filename=\"Orders.xlsx\"";
        return isUploaded;
    }
    [HttpGet]
    public IActionResult History(int id)
    {
        var customers = _customerService.GetHistory(id);

        return Json(new { obj = customers });
    }
}
