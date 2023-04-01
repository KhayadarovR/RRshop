using Microsoft.AspNetCore.Mvc;
using RRshop.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using RRshop.ViewModels;

namespace RRshop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly rrshopContext _context;

        public HomeController(ILogger<HomeController> logger, rrshopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            HomeViewModel viewModel = new HomeViewModel();

            viewModel.Categories = _context.Categories.ToList();
            var allProds = _context.Prods.Include(p => p.Category).ToList();
            
            
            for (int i = 0; i < allProds.Count; i++)
            {
                List<float> sizeValueList = new();
                List<Size> sizeList = _context.Sizes.Where(s => s.ProdId == allProds[i].Id).ToList();
                sizeList.ForEach(s => sizeValueList.Add(s.Size1));
                
                viewModel.DetailProds.Add(new DetailProdViewModel()
                {
                    Prod = allProds[i],
                    SizeList = sizeValueList
                });
            }
            
            return View(viewModel);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}