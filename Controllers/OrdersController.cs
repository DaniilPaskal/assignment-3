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
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: /order
        [Route("order")]
        [HttpPost]
        public async Task<IActionResult> Create(int userId)
        {
            DateTime date = DateTime.Now;
            Random random = new Random();
            UsersController usersController = new UsersController(_context);
            CartsController cartsController = new CartsController(_context);
            List<Product> products = new List<Product>();
            List<int> quantities = new List<int>();
            List<Product> purchaseHistory = new List<Product>();
            double totalCost = 0;
            User user;
            Cart cart;
            Order order;

            // Get user purchase history
            var userResult = await usersController.GetUserById(userId);
            user = userResult as User;
            purchaseHistory = user.PurchaseHistory;
            
            // Get products and quantities in cart
            var cartResult = await cartsController.GetCartById(userId);
            cart = cartResult as Cart;
            products = cart.Products;
            quantities = cart.Quantities;

            // Iterate through products
            for (int i = 0; i < products.Count; i++) {
                double cost, shippingCost = 0;

                // If product not in purchase history, add to history
                if (purchaseHistory.IndexOf(products[i]) < 0) {
                    purchaseHistory.Add(products[i]);
                }

                // Get product cost and shipping cost
                cost = products[i].Cost;
                shippingCost = products[i].ShippingCost;

                // Add product cost multiplied by product quantity to total cost
                totalCost += (cost * quantities[i]) + shippingCost;
            }

            // Update user purchase history
            user.PurchaseHistory = purchaseHistory;
            usersController.Edit(user);

            // Clear cart
            cartsController.ClearCart(userId);

            // Create order
            order = new Order();
            order.Id = random.Next(1, 10000);
            order.Date = date;
            order.Cost = totalCost;
            order.UserId = userId;

            // Record order
            if (usersController.UserExists(userId)) {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return Ok($"Recorded order.");
            }

            return BadRequest($"Error recording order.");
        }
    }
}
