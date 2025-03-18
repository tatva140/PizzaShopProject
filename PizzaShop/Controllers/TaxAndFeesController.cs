using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Service;

namespace PizzaShop.Controllers;

public class TaxAndFeesController:Controller
{
    private readonly TaxAndFeesService _taxAndFeesService;
    public TaxAndFeesController(TaxAndFeesService taxAndFeesService){
        _taxAndFeesService=taxAndFeesService;
    }

    public ActionResult Index(){
        return View();
    }

[HttpGet]
    public IActionResult TaxAndFees(string search, int pageNumber=1,int selectedPage=2){
        // List<Tax> taxes  = _taxAndFeesService.GetTaxes();
        var (taxes, totalRecords) = _taxAndFeesService.GetTaxes(search, pageNumber, selectedPage);
        int totalPage = (int)Math.Ceiling((double)totalRecords / selectedPage);
        TaxAndFeesViewModel taxAndFeesViewModel = new TaxAndFeesViewModel
        {
            taxes = taxes,
            PageNumber = pageNumber,
            TotalPages = totalPage,
            SelectedPage = selectedPage,
            
        };
        ViewBag.taxSearch=search;
        
        return PartialView("_TaxAndFees", taxAndFeesViewModel);
    }
}
