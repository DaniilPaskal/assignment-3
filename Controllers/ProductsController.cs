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
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: /add-product
        [Route("add-product")]
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            product.Rating = 0;

            if (true) {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return Ok($"{product.Name} has been saved in the database.");
            }

            return BadRequest("Error saving {product.Name}.");
        }

        // GET: /get-products
        [Route("get-products")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _context.Products.ToListAsync());
        }

        // PUT: /update-product
        [Route("update-product")]
        [HttpPut]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound($"Failed to update {product.Name}.");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok($"Updated {product.Name}.");
        }

        // PUT: /update-rating
        [Route("update-rating")]
        [HttpPut]
        public async Task<IActionResult> EditRating(Product product)
        {
            CommentsController commentsController = new CommentsController(_context);
            List<Comment> comments;
            int ratingCount = 0;
            double ratingSum = 0;
            double rating = 0;

            // Get all comments associated with product
            var commentsResult = await commentsController.GetCommentsByProduct(product.Id);
            comments = commentsResult as List<Comment>;

            // Get sum and count of ratings
            ratingCount = comments.Count;
            for (int i = 0; i < ratingCount; i++) {
                ratingSum += comments[i].Rating;
            }

            // Calculate average rating from sum and count
            rating = ratingSum / ratingCount;
            product.Rating = rating;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound($"Error updating product.");
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return Ok($"Rating has been updated to {product.Rating}.");
        }

        // DELETE: /delete-product
        [Route("delete-product")]
        [HttpDelete]
        public async Task<IActionResult> DeleteConfirmed(Product product)
        {
			CommentsController commentsController = new CommentsController(_context);
			
			// Delete product
            var deletedProduct = await _context.Products.FindAsync(product.Id);
            _context.Products.Remove(deletedProduct);
            await _context.SaveChangesAsync();
			
			// Delete product comments
			commentsController.DeleteCommentsByProduct(product.Id);
			
            return Ok($"Deleted product named {deletedProduct.Name}.");
        }

        // Get product with specific ID
        public async Task<IActionResult> GetProductById(int productId) {
            return Ok(_context.Products.Single(e => e.Id == productId));
        }

        // Check if product exists
        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
