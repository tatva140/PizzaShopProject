using Microsoft.AspNetCore.Mvc;

namespace PizzaShop.Controllers;

public class MenuController:Controller
{
    public ActionResult Index()
    {
        return View();
    }
}
