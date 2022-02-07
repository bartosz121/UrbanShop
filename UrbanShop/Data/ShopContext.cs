using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrbanShop.Models;

namespace UrbanShop.Data
{
    public class ShopContext : IdentityDbContext<ShopUser>
    {
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductImage> ProductImages { get; set; }

        public ShopContext(DbContextOptions<ShopContext> options) : base(options)
        {
        }
    }
}
