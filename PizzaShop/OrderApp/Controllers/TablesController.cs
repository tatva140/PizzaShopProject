using System.Text.Json.Nodes;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Services.Service;

namespace PizzaShop.OrderApp;

[Authorize(Roles="Account Manager")]

public class TablesController : Controller
{
    private readonly OrderAppTablesService _orderAppTablesService;
    private readonly TableAndSectionService _tableAndSectionService;

    public TablesController(OrderAppTablesService orderAppTablesService, TableAndSectionService tableAndSectionService)
    {
        _orderAppTablesService = orderAppTablesService;
        _tableAndSectionService = tableAndSectionService;
    }

    public IActionResult Index()
    {
        ViewData["Icon"] = "false";
        List<Section> sections = _tableAndSectionService.GetSections();
        OrderAppTablesViewModel orderAppTablesViewModels = _orderAppTablesService.GetTablesAndSections(sections);
        return View("~/OrderApp/Views/Tables/Index.cshtml", orderAppTablesViewModels);
    }
    public IActionResult WaitingTokenList(int id)
    {
        ViewData["Icon"] = "false";
        WaitingTokenListViewModel waitingTokenListViewModels = _orderAppTablesService.WaitingTokenList(id);
        return View("~/OrderApp/Views/Shared/_WaitingTokenListView.cshtml", waitingTokenListViewModels);
    }
    public IActionResult CustomerDetails(string email)
    {
        ViewData["Icon"] = "false";
        OrderAppCustomerViewModel orderAppCustomerViewModel = new OrderAppCustomerViewModel();
        if (email != null)
        {
            orderAppCustomerViewModel = _orderAppTablesService.CustomerDetails(email);
            if (orderAppCustomerViewModel == null)
            {
                return Ok(new { message = "Error" });

            }
            return View("~/OrderApp/Views/Shared/_CustomerDetails.cshtml", orderAppCustomerViewModel);
        }

        return View("~/OrderApp/Views/Shared/_CustomerDetails.cshtml", orderAppCustomerViewModel);

    }
    public IActionResult WaitingTokenCustomerDetails(int waitingTokenId)
    {
        ViewData["Icon"] = "false";
        OrderAppCustomerViewModel orderAppCustomerViewModel = _orderAppTablesService.WaitingTokenCustomerDetails(waitingTokenId);
        if (orderAppCustomerViewModel == null)
        {
            return Ok(new { message = "Error" });

        }
        return View("~/OrderApp/Views/Shared/_CustomerDetails.cshtml", orderAppCustomerViewModel);
    }

    [HttpPost]
    public IActionResult AssignTable(OrderAppCustomerViewModel orderAppCustomerViewModel , string selectedTables)
    {
        ViewData["Icon"] = "false";
        List<int> selectedTables1 = JsonConvert.DeserializeObject<List<int>>(selectedTables);
        orderAppCustomerViewModel.selectedTables=selectedTables1;
        CustomErrorViewModel added = _orderAppTablesService.AssignTable(orderAppCustomerViewModel);
        if(added.Status==false){
            TempData["error"]=added.Message;
        }else{
            TempData["success"]=added.Message;
        }
        return Ok(new { message = "Added" });
    }
    [HttpPost]
    public IActionResult AddWaitingToken(OrderAppCustomerViewModel orderAppCustomerViewModel)
    {
        ViewData["Icon"] = "false";
        CustomErrorViewModel added = _orderAppTablesService.AddWaitingToken(orderAppCustomerViewModel);
        return Ok(new { message = added.Message, added.Status  });
    }
}
