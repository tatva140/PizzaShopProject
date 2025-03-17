using System.Text.Json.Nodes;
using DAL.Models;
using DAL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services.Service;

namespace PizzaShop.Controllers;

public class TablesAndSectionsController : Controller
{
    private readonly TableAndSectionService _tableAndSectionService;

    public TablesAndSectionsController(TableAndSectionService tableAndSectionService)
    {
        _tableAndSectionService = tableAndSectionService;
    }
    public ActionResult Index()
    {
        return View();
    }
    [HttpGet]
    public IActionResult TablesAndSections(int sectionId,string search, int pageNumber = 1, int pageSize = 2, int selectedPage = 2)
    {
        List<Section> sections = _tableAndSectionService.GetSections();
        var (tables, totalRecords) = _tableAndSectionService.GetTables(sectionId,search, pageNumber, selectedPage);
        int totalPage = (int)Math.Ceiling((double)totalRecords / selectedPage);
        TablesAndSectionViewModel tablesAndSectionViewModel = new TablesAndSectionViewModel
        {
            sections = sections,
            tables = tables,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPage,
            SelectedPage = selectedPage,
            ShowList = sectionId
        };
        return PartialView("_Sections", tablesAndSectionViewModel);
    }
    [HttpGet]
    public IActionResult Tables(int sectionId,string search, int pageNumber = 1, int pageSize = 2, int selectedPage = 2)
    {
        List<Section> sections = _tableAndSectionService.GetSections();
        var (tables, totalRecords) = _tableAndSectionService.GetTables(sectionId,search, pageNumber, selectedPage);
        int totalPage = (int)Math.Ceiling((double)totalRecords / selectedPage);
        TablesAndSectionViewModel tablesAndSectionViewModel = new TablesAndSectionViewModel
        {
            sections = sections,
            tables = tables,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPage,
            SelectedPage = selectedPage,
            ShowList = sectionId
        };
        ViewBag.TableSearch=search;
        return PartialView("_Tables", tablesAndSectionViewModel);
    }
    [HttpPost]
    public IActionResult AddSection(Section section)
    {
        int isAdded = _tableAndSectionService.AddSection(section);
        if (isAdded != 0)
        {
            TempData["success"] = "Section Added Successfully";
        }
        else
        {
            TempData["error"] = "Section Cannot be added";
        }
        return RedirectToAction("TablesAndSections", new { sectionId = isAdded });

    }
    [HttpPost]
    public IActionResult DeleteSection([FromBody] int id)
    {
        bool isDeleted = _tableAndSectionService.DeleteSection(id);
        if (isDeleted)
        {
            TempData["success"] = "Section Deleted Successfully";
        }
        else
        {
            TempData["error"] = "Section Cannot be deleted";
        }
        return RedirectToAction("TablesAndSections", "TablesAndSections");
    }

    [HttpGet]
    public IActionResult GetSectionDetails(int id)
    {
        Section section = _tableAndSectionService.GetSectionDetails(id);
        return Ok(new { section = section });
    }
    [HttpPost]
    public IActionResult EditSection(Section section)
    {
        int isEdited = _tableAndSectionService.EditSection(section);
        if (isEdited != 0)
        {
            TempData["success"] = "Section Edited Successfully";
        }
        else
        {
            TempData["error"] = "Section Cannot be edited";
        }
        return RedirectToAction("TablesAndSections", new { sectionId = isEdited });
    }
    [HttpPost]
    public IActionResult AddTable(Table table)
    {
        int isAdded = _tableAndSectionService.AddTable(table);
        if (isAdded != 0)
        {
            TempData["success"] = "Table Added Successfully";
        }
        else
        {
            TempData["error"] = "Table Cannot be added";
        }
        return RedirectToAction("Tables", new { sectionId = isAdded });

    }
    [HttpGet]
    public IActionResult GetTableDetails(int id)
    {
        Table table = _tableAndSectionService.GetTableDetails(id);
        return Ok(new { table = table });
    }
    [HttpPost]
    public IActionResult EditTable(Table table)
    {
        int isEdited = _tableAndSectionService.EditTable(table);
        if (isEdited != 0)
        {
            TempData["success"] = "Table Edited Successfully";
        }
        else
        {
            TempData["error"] = "Table Cannot be edited";
        }
        return RedirectToAction("Tables", new { sectionId = isEdited });
    }
    [HttpPost]
    public IActionResult DeleteTables([FromBody] JsonArray ids)
    {
        bool isDeleted = _tableAndSectionService.DeleteTables(ids);
        if (isDeleted)
        {
            TempData["success"] = "Selected Tables Deleted Successfully";
        }
        else
        {
            TempData["error"] = "Tables cannot be deleted";
        }
        return Ok(new { redirectUrl = @Url.Action("Index", "TablesAndSections") });
    }
    [HttpPost]
    public IActionResult DeleteTable([FromBody] int id)
    {
        int isDeleted = _tableAndSectionService.DeleteTable(id);
        if (isDeleted!=0)
        {
            TempData["success"] = "Table Deleted Successfully";
        }
        else
        {
            TempData["error"] = "Table Cannot be deleted";
        }
        return RedirectToAction("Tables", new { sectionId = isDeleted });

    }
}
