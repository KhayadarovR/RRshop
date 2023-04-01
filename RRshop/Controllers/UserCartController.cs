using Microsoft.AspNetCore.Mvc;
using RRshop.Models;

namespace RRshop.Controllers;

public class UserCartController: Controller
{
    private readonly rrshopContext _context;
    
    public UserCartController(rrshopContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
}