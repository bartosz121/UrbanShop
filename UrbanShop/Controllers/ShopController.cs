using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrbanShop.Data;
using UrbanShop.ViewModels;

namespace UrbanShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopContext _context;

        public ShopController(ShopContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                .Where(c => c.IsVisible)
                .Include(c => c.Products.Where(p => p.IsVisible).Take(4))
                .ThenInclude(p => p.ProductImages.Take(1))
                .AsSplitQuery()
                .ToListAsync();

            return View(categories);
        }

        public async Task<IActionResult> Category(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Products.Where(p => p.IsVisible))
                .ThenInclude(p => p.ProductImages.Take(1))
                .AsSplitQuery()
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (category == null || !category.IsVisible)
            {
                return NotFound();
            }

            return View(category);
        }

        public async Task<IActionResult> Product(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.Category)
                .AsSplitQuery()
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (product == null || !product.IsVisible || !product.Category.IsVisible)
            {
                return NotFound();
            }

            var relatedProducts = await _context.Products
                .Where(p => p.Id != id && p.IsVisible && p.CategoryId == product.CategoryId)
                .Include(p => p.ProductImages.Take(1)).Take(4)
                .AsSplitQuery().ToListAsync();

            var context = new ProductPageViewModel
            {
                product = product,
                relatedProducts = relatedProducts
            };

            return View(context);
        }

        [Authorize(Roles = "ShopUser, Admin")]
        public IActionResult Cart()
        {
            return View();
        }
    }
}
