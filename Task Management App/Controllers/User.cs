using Microsoft.AspNetCore.Mvc;

namespace Task_Management_App.Controllers;

public class User : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }


}