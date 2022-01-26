using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using UrbanShop.Data;
using UrbanShop.Models;

namespace UrbanShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShopContext _context;

        public HomeController(ILogger<HomeController> logger, ShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(p => p.ProductImages.Take(1))
                .Where(p => p.IsVisible && p.Category.IsVisible)
                .Take(8).ToListAsync();

            return View(products);
        }

        // TODO authentication
        public IActionResult AdminPanel()
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
