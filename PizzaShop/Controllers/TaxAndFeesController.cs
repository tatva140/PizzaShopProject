using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Service;

namespace PizzaShop.Controllers;

public class TaxAndFeesController : Controller
{
    private readonly TaxAndFeesService _taxAndFeesService;
    public TaxAndFeesController(TaxAndFeesService taxAndFeesService)
    {
        _taxAndFeesService = taxAndFeesService;
    }

    public ActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult TaxAndFees(string search, int pageNumber = 1, int selectedPage = 2)
    {
        var (taxes, totalRecords) = _taxAndFeesService.GetTaxes(search, pageNumber, selectedPage);
        int totalPage = (int)Math.Ceiling((double)totalRecords / selectedPage);
        TaxAndFeesViewModel taxAndFeesViewModel = new TaxAndFeesViewModel
        {
            taxes = taxes,
            PageNumber = pageNumber,
            TotalPages = totalPage,
            SelectedPage = selectedPage,
        };
        ViewBag.taxSearch = search;

        return PartialView("_TaxAndFees", taxAndFeesViewModel);
    }

    [HttpPost]
    public IActionResult AddTax(Tax tax)
    {
        int isAdded = _taxAndFeesService.AddTax(tax);
        if (isAdded != 0)
        {
            TempData["success"] = "Table Added Successfully";
        }
        else
        {
            TempData["error"] = "Table Cannot be added";
        }
        return RedirectToAction("TaxAndFees");

    }
    [HttpPost]
    public IActionResult DeleteTax([FromBody] int id)
    {
        CustomErrorViewModel isDeleted = _taxAndFeesService.DeleteTax(id);
        if (isDeleted.Status)
        {
            TempData["success"] = isDeleted.Message;
        }
        else
        {
            TempData["error"] = isDeleted.Message;
        }
        return Ok(new { message = "Deleted" });
    }
    [HttpGet]
    public IActionResult GetTaxDetails(int id)
    {
        Tax tax = _taxAndFeesService.GetTaxDetails(id);
        return Ok(new { tax = tax });
    }
    [HttpPost]
    public IActionResult EditTax(TaxAndFeesViewModel taxAndFeesViewModel)
    {
        CustomErrorViewModel customErrorViewModel = _taxAndFeesService.EditTax(taxAndFeesViewModel);
        if (customErrorViewModel.Status)
        {
            TempData["success"] = customErrorViewModel.Message;
        }
        else
        {
            TempData["error"] = customErrorViewModel.Message;
        }
        return Ok(new { message = "Edited" });

    }
}
