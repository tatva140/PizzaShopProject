using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Services.Service;

namespace PizzaShop.OrderApp;


 public class TablesController : Controller
    {
        private readonly OrderAppTablesService _orderAppTablesService;
        private readonly TableAndSectionService _tableAndSectionService;

        public TablesController(OrderAppTablesService orderAppTablesService,TableAndSectionService tableAndSectionService){
            _orderAppTablesService=orderAppTablesService;
            _tableAndSectionService=tableAndSectionService;
        }
        public IActionResult Index()
        {
            List<Section> sections=_tableAndSectionService.GetSections();
            OrderAppTablesViewModel orderAppTablesViewModels = _orderAppTablesService.GetTablesAndSections(sections);
            return View("~/OrderApp/Views/Tables/Index.cshtml",orderAppTablesViewModels); 
        }
    }
