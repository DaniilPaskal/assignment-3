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
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: /register
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
			CartsController cartsController = new CartsController(_context);
            Random random = new Random();
			
            user.Id = random.Next(1, 10000);
			user.PurchaseHistory = new List<Product>();
			
			// Create user cart
			cartsController.CreateCart(user);

            if (true) {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return Ok($"{user.Name} has been saved in the database.");
            }

            return BadRequest($"Error saving {user.Name}.");
        }
		
		// GET: /get-users
        [Route("get-users")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        // GET: /login
        [Route("login")]
        [HttpGet]
        public async Task<IActionResult> Login(String email, String password)
        {
            User user = await _context.Users.Where(e => e.Email == email && e.Password == password).SingleOrDefaultAsync();

            if (user != null) {
                return Ok($"User authenticated.");
            } else {
                return NotFound($"User not found.");
            }
        }

        // PUT: /update-user
        [Route("update-user")]
        [HttpPut]
        public async Task<IActionResult> Edit(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound($"Failed to update {user.Name}.");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok($"Updated {user.Name}.");
        }

        // DELETE: /delete-user
        [Route("delete-user")]
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(User user)
        {
            var deletedUser = await _context.Users.FindAsync(user.Id);
            _context.Users.Remove(deletedUser);
            await _context.SaveChangesAsync();
            return Ok($"Deleted user with email {user.Email}.");
        }

        // Get user with specific ID
        public async Task<IActionResult> GetUserById(int userId) {
            return Ok(_context.Users.Single(e => e.Id == userId));
        }

        // Check if user exists
        public bool UserExists(int userId)
        {
            return _context.Users.Any(e => e.Id == userId);
        }
    }
}
