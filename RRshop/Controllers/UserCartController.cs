using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        int uId = int.Parse(HttpContext.User.FindFirst("id").Value);
        var prods = _context.UserCarts.Where(c => c.UserId == uId)
            .Include(p => p.Prod).ToList();

        for (int i = 0; i < prods.Count; i++)
        {
            prods[i].Prod.Category = _context.Categories.First(p => p.Id == prods[i].Prod.CategoryId);
        }
        return View(prods);
    }
}