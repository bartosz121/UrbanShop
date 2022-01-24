
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrbanShop.ViewComponents
{
    public class AspRouteBtn : ViewComponent
    {
        public class AspRouteBtnContext{
            public string controller { get; set; }
            public string action { get; set; }
            public string wrapperClass { get; set; }
            public string btnText { get; set; }
        }

        public IViewComponentResult Invoke(string controller, string action, string wrapClass, string btnText)
        {
            var context = new AspRouteBtnContext();
            context.controller = controller;
            context.action = action;
            context.wrapperClass = wrapClass;
            context.btnText = btnText;

            return View("Default", context);
        }
    }
}
