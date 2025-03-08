using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Inklusive.Models;

namespace Inklusive.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }
    public IActionResult index()
    {
        return View();
    }
    public IActionResult EmployeeHome()
    {
        return View();
    }
    public IActionResult AdminHome()
    {
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}
