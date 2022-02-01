using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UrbanShop.Data;
using UrbanShop.Models;
using UrbanShop.ViewModels;

namespace UrbanShop.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("/admin/[controller]/[action]")]
    public class ProductImagesController : Controller
    {
        private readonly ShopContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductImagesController(ShopContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: ProductImages1
        public async Task<IActionResult> Index(string searchInput, string searchId)
        {
            IQueryable<ProductImage> productImagesQuery = _context.ProductImages.Include(p => p.Product);

            if (!string.IsNullOrEmpty(searchInput))
            {
                productImagesQuery = productImagesQuery.Where(x => x.Product.Name.Contains(searchInput));
            }

            if (!string.IsNullOrEmpty(searchId))
            {
                productImagesQuery = productImagesQuery.Where(x => x.ImageUrl.Contains($"pid{searchId}"));
            }

            return View(await productImagesQuery.ToListAsync());
        }

        // GET: ProductImages1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImage = await _context.ProductImages
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productImage == null)
            {
                return NotFound();
            }

            return View(productImage);
        }

        // GET: ProductImages1/Create
        public IActionResult Create()
        {
            ProductImageViewModel ProductImageVM = new ProductImageViewModel();
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View(ProductImageVM);
        }

        // POST: ProductImages1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId","Images")] ProductImageViewModel ProductImageVM)
        {
            foreach (var item in ProductImageVM.Images)
            {
                string stringFileName = UploadFile(item, ProductImageVM.ProductId);
                var productImage = new ProductImage
                {
                    ImageUrl = stringFileName,
                    ProductId = ProductImageVM.ProductId
                };

                await _context.ProductImages.AddAsync(productImage);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        // TODO validation (file extension)
        private string UploadFile(IFormFile file, int productId)
        {
            string fileName = null;

            if (file != null)
            {
                string uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, "product_images", $"productId_{productId}");

                if (!Directory.Exists(uploadPath))
                {
                    try
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"UploadFile failed: {e}");
                    }
                }

                fileName = $"pid{productId}-"+ Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadPath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

            }

            return fileName;

        }

        // GET: ProductImages1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImage = await _context.ProductImages
                .Include(p => p.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productImage == null)
            {
                return NotFound();
            }

            return View(productImage);
        }

        // POST: ProductImages1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productImage = await _context.ProductImages.FindAsync(id);
            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductImageExists(int id)
        {
            return _context.ProductImages.Any(e => e.Id == id);
        }
    }
}
