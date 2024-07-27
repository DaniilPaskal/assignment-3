using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using assignment_3.Data;
using assignment_3.Models;

namespace assignment_3.Controllers
{
	[Route("")]
    [ApiController]
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /get-cart
        [Route("get-cart")]
        [HttpGet]
        public async Task<IActionResult> GetCartById(int userId)
        {
            return Ok(await _context.Carts.Where(e => e.UserId == userId).SingleOrDefaultAsync());
        }

        // PUT: /update-cart
        [Route("update-cart")]
        [HttpPut]
        public async Task<IActionResult> Edit(int userId, Product product, int quantity)
        {
            List<Product> products = new List<Product>();
            List<int> quantities = new List<int>();
            Cart cart;
            int index;

            // Get existing cart products and quantities
            var cartResult = await GetCartById(userId);
            cart = cartResult as Cart;
            if (cart != null) {
                products = cart.Products;
                quantities = cart.Quantities;
            }

            //Check if product in cart
            index = cart.Products.IndexOf(product);
            if (index >= 0) {
                // If product in cart, update existing quantity
                quantities[index] = quantity;
            } else {
                // If product not in cart, add new product and quantity
                products.Add(product);
                quantities.Add(quantity);
            }

            // If quantity is 0, remove product and quantity from respective arrays
            if (quantity == 0) {
                products.RemoveAt(index);
                quantities.RemoveAt(index);
            }

            // Replace cart products and quantities with new arrays
            cart.Products = products;
            cart.Quantities = quantities;

            // Update cart
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartExists(cart.Id))
                    {
                        return NotFound($"Failed to update cart.");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return Ok($"Updated cart.");
        }

        // Check if cart exists
        private bool CartExists(int id)
        {
            return _context.Carts.Any(e => e.Id == id);
        }
		
		// Create cart
		public async void CreateCart(int userId) {
			Random random = new Random();
			Cart cart = new Cart();
			
			cart.Id = random.Next(1, 10000);
			cart.UserId = userId;
			cart.Products = new List<Product>();
			cart.Quantities = new List<int>();
			
			_context.Add(cart);
            await _context.SaveChangesAsync();
		}

        // Clear cart
        public async void ClearCart(int userId) {
            var cartResult = await GetCartById(userId);
            Cart cart = cartResult as Cart;
            
            cart.Products.Clear();
            cart.Quantities.Clear();

            _context.Update(cart);
            await _context.SaveChangesAsync();
        }
    }
}
