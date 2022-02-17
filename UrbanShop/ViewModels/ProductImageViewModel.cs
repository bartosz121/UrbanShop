using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrbanShop.Models;

namespace UrbanShop.ViewModels
{
    public class ProductImageViewModel
    {
        public List<IFormFile> Images { get; set; }

        public int ProductId { get; set; }
    }
}
