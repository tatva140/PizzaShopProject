using Microsoft.AspNetCore.Mvc;

namespace PizzaShop.Controllers;

public class MenuController:Controller
{
    public ActionResult Index()
    {
        return View();
    }

[HttpGet]
    public IActionResult MenuItems()
    {
        return PartialView("_MenuItems");
    }
}
