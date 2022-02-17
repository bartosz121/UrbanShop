using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrbanShop.ApiModels;
using Newtonsoft.Json;
using Stripe;
using UrbanShop.Data;

namespace UrbanShop.Controllers
{
    [Route("api/create-payment-intent")]
    [ApiController]
    public class PaymentIntentApiController : Controller
    {
        private readonly ShopContext _context;

        public PaymentIntentApiController(ShopContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> Create(PaymentIntentCreateRequest request)
        {
            var orderPrice = await GetOrderPrice(request.items);
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
            {
                Amount = orderPrice,
                Currency = "eur",
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            });

            return Json(new { clientSecret = paymentIntent.ClientSecret });
        }

        private async Task<long> GetOrderPrice(List<CartItemApiModel> items)
        {
            long totalPrice = 0;
            foreach (var item in items)
            {
                totalPrice += Convert.ToInt64(Decimal.Multiply(item.TotalPrice, 100));
                var cartItem = await _context.CartItem.FindAsync(item.Id);
                if (cartItem != null)
                {
                    _context.CartItem.Remove(cartItem);
                }
            }

            await _context.SaveChangesAsync();

            return totalPrice;

        }

        public class PaymentIntentCreateRequest
        {
            [JsonProperty("items")]
            public List<CartItemApiModel> items { get; set; }

        }
    }
}
