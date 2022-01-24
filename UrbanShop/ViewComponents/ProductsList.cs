using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrbanShop.Data;
using UrbanShop.Models;

namespace UrbanShop.ViewComponents
{
    public class ProductsList : ViewComponent
    {

        public class ProductsListContext
        {
            public Category category { get; set; }
            public bool seeMoreVisible { get; set; }
        }


        public IViewComponentResult Invoke(Category category, bool seeMore)
        {
            var context = new ProductsListContext();
            context.category = category;
            context.seeMoreVisible = seeMore;

            return View("Default", context);
        }
    }
}
