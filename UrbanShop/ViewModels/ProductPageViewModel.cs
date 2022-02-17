using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrbanShop.Models;

namespace UrbanShop.ViewModels
{
    public class ProductPageViewModel
    {
        public Product product { get; set; }
        public List<Product> relatedProducts { get; set; }
    }
}
