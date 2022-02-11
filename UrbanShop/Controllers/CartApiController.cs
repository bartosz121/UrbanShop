using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbanShop.ApiModels;
using UrbanShop.Data;
using UrbanShop.Models;

namespace UrbanShop.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartApiController : ControllerBase
    {
        private readonly ShopContext _context;

        public CartApiController(ShopContext context)
        {
            _context = context;
        }

        public async Task<CartItemApiModel> ConvertToApiModel(CartItem cartItem)
        {
            var product = await _context.Products
                .Include(p => p.ProductImages.Take(1))
                .Where(p => p.Id == cartItem.ProductId).FirstAsync();

            var thumbnail = product.GetThumbnailUrl();

            return new CartItemApiModel
            {
                Id = cartItem.Id,
                ProductId = product.Id,
                ProductName = product.Name,
                ProductThumbnail = thumbnail,
                Quantity = cartItem.Quantity,
                Price = product.Price,
                TotalPrice = product.Price * cartItem.Quantity,
            };
        }

        // GET: api/cart/user/1
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<CartItemApiModel>>> GetUserCartItems(Guid userId)
        {
            var cartItems = await _context.CartItem.Where(i => i.UserId == userId).ToListAsync();
            var items = new List<CartItemApiModel>();

            foreach (var item in cartItems)
            {
                items.Add(await ConvertToApiModel(item));
            }

            return items;
        }

        // GET: api/cart/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartItemApiModel>> GetCartItem(int id)
        {
            var cartItem = await _context.CartItem.FindAsync(id);

            if (cartItem == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(cartItem.ProductId);

            return new CartItemApiModel 
            { 
                Id=cartItem.Id,
                ProductName = product.Name,
                Quantity = cartItem.Quantity,
                Price = product.Price,
                TotalPrice = product.Price * cartItem.Quantity,
            };
        }

        // PUT: api/cart/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartItem(int id, CartItem cartItem)
        {
            if (id != cartItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(cartItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/cart
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CartItemApiModel>> PostCartItem(CartItem cartItem)
        {
            var alreadyInCart = await _context.CartItem.Where(i => i.UserId == cartItem.UserId && i.ProductId == cartItem.ProductId).FirstOrDefaultAsync();

            if (alreadyInCart != null)
            {
                alreadyInCart.Quantity += cartItem.Quantity;
                await PutCartItem(alreadyInCart.Id, alreadyInCart);
                return await GetCartItem(alreadyInCart.Id);
            }


            _context.CartItem.Add(cartItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCartItem", new { id = cartItem.Id }, cartItem);
        }

        // DELETE: api/CartItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCartItem(int id)
        {
            var cartItem = await _context.CartItem.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }

            _context.CartItem.Remove(cartItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        //// DELETE: api/cart/user/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteUserCart(int userd)
        //{
        //    var cartItem = await _context.CartItem.FindAsync(id);
        //    if (cartItem == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.CartItem.Remove(cartItem);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool CartItemExists(int id)
        {
            return _context.CartItem.Any(e => e.Id == id);
        }
    }
}
