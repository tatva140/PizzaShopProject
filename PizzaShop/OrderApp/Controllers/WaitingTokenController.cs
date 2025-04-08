using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Service;

namespace PizzaShop.OrderApp.Controllers;

public class WaitingTokenController:Controller
{
    private readonly WaitingTokenService _waitingTokenService;
    private readonly TableAndSectionService _tableAndSectionService;
    public WaitingTokenController(WaitingTokenService waitingTokenService, TableAndSectionService tableAndSectionService)
    
    {
        _waitingTokenService = waitingTokenService;
        _tableAndSectionService = tableAndSectionService;

    }

    //  public IActionResult Index()
    // {
    //     ViewData["Icon"] = "false";
    //     OrderAppTablesViewModel orderAppTablesViewModel = _waitingTokenService.GetSections();
    //     return View("~/OrderApp/Views/WaitingToken/Index.cshtml", orderAppTablesViewModels);
    // }

}
