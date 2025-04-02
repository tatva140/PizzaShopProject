using System.Text.Json.Nodes;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Services.Service;

namespace PizzaShop.OrderApp;


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
        List<Section> sections = _tableAndSectionService.GetSections();
        OrderAppTablesViewModel orderAppTablesViewModels = _orderAppTablesService.GetTablesAndSections(sections);
        return View("~/OrderApp/Views/Tables/Index.cshtml", orderAppTablesViewModels);
    }
    public IActionResult WaitingTokenList(int id)
    {
        WaitingTokenListViewModel waitingTokenListViewModels = _orderAppTablesService.WaitingTokenList(id);
        return View("~/OrderApp/Views/Shared/WaitingTokenListView.cshtml", waitingTokenListViewModels);
    }
    public IActionResult CustomerDetails(string email)
    {
        OrderAppCustomerViewModel orderAppCustomerViewModel = new OrderAppCustomerViewModel();
        if (email != null)
        {
            orderAppCustomerViewModel = _orderAppTablesService.CustomerDetails(email);
            if (orderAppCustomerViewModel == null)
            {
                return Ok(new { message = "Error" });

            }
            return View("~/OrderApp/Views/Shared/CustomerDetails.cshtml", orderAppCustomerViewModel);
        }

        return View("~/OrderApp/Views/Shared/CustomerDetails.cshtml", orderAppCustomerViewModel);

    }
    public IActionResult WaitingTokenCustomerDetails(string email)
    {
        OrderAppCustomerViewModel orderAppCustomerViewModel = _orderAppTablesService.WaitingTokenCustomerDetails(email);
        if (orderAppCustomerViewModel == null)
        {
            return Ok(new { message = "Error" });

        }
        return View("~/OrderApp/Views/Shared/CustomerDetails.cshtml", orderAppCustomerViewModel);
    }

    [HttpPost]
    public IActionResult AssignTable([FromBody] JsonObject obj)
    {
        string data = obj.ToJsonString();
        AssignCustomerTablesViewModel assignCustomerTablesViewModel = JsonConvert.DeserializeObject<AssignCustomerTablesViewModel>(data);
        CustomErrorViewModel added = _orderAppTablesService.AssignTable(assignCustomerTablesViewModel);
        if (added.Status == false)
        {
            TempData["error"] = added.Message;
        }
        else
        {
            TempData["success"] = added.Message;
        }
        return Ok(new { message = added.Message, added.Status });
    }
}
