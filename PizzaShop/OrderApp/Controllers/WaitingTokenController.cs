using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Service;

namespace PizzaShop.OrderApp.Controllers;

[Authorize(Roles="Account Manager")]

public class WaitingTokenController:Controller
{
    private readonly WaitingTokenService _waitingTokenService;
    private readonly TableAndSectionService _tableAndSectionService;
    private readonly OrderAppTablesService _orderAppTablesService;
    public WaitingTokenController(WaitingTokenService waitingTokenService, TableAndSectionService tableAndSectionService,OrderAppTablesService orderAppTablesService)
    
    {
        _waitingTokenService = waitingTokenService;
        _tableAndSectionService = tableAndSectionService;
        _orderAppTablesService=orderAppTablesService;

    }


     public IActionResult Index()
    {
        ViewData["Icon"] = "false";
        OrderAppTablesViewModel orderAppTablesViewModel = _waitingTokenService.GetSections();
        return View("~/OrderApp/Views/WaitingToken/Index.cshtml", orderAppTablesViewModel);
    }
    public IActionResult WaitingTokenList(int id)
    {
        ViewData["Icon"] = "false";
        WaitingTokenListViewModel waitingTokenListViewModels = _orderAppTablesService.WaitingTokenList(id);
        return View("~/OrderApp/Views/Shared/_WaitingListView.cshtml", waitingTokenListViewModels);
    }

[HttpPost]
    public IActionResult DeleteWaitingToken(int id){
        CustomErrorViewModel customErrorViewModel=_waitingTokenService.DeleteWaitingToken(id);
        return Ok(new {message=customErrorViewModel.Message,status=customErrorViewModel.Status});
    }

    public IActionResult AvailableTables(int id){
        List<Table> tables=_waitingTokenService.AvailableTables(id);
        return Ok(new {tables});
    }

     [HttpPost]
    public IActionResult AssignTable(OrderAppCustomerViewModel orderAppCustomerViewModel , string selectedTables)
    {
        ViewData["Icon"] = "false";
        List<int> selectedTables1 = JsonConvert.DeserializeObject<List<int>>(selectedTables);
        orderAppCustomerViewModel.selectedTables=selectedTables1;
        CustomErrorViewModel added = _waitingTokenService.AssignTable(orderAppCustomerViewModel);
      
        return Ok(new { message = added.Message,Status=added.Status });
    }

    
}
