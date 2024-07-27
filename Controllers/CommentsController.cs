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
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: /comment
        [Route("comment")]
        [HttpPost]
        public async Task<IActionResult> Create(Comment comment)
        {
            Random random = new Random();
            comment.Id = random.Next(1, 10000);

            if (comment.Text.Length > 0) {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return Ok($"Comment saved in the database.");
            }

            return BadRequest($"Error saving comment.");
        }

        // GET: /get-comments
        [Route("get-comments")]
        [HttpGet]
        public async Task<IActionResult> GetCommentsByProduct(int productId)
        {
            return Ok(await _context.Comments.Where(e => e.ProductId == productId).ToListAsync());
        }
		
		// Delete comments associated with a product
		public async void DeleteCommentsByProduct(int productId) {
			List<Comment> deletedComments = await _context.Comments.Where(e => e.ProductId == productId).ToListAsync();
			
			for (int i = 0; i < deletedComments.Count; i++) {
				_context.Comments.Remove(deletedComments[i]);
			}
			
			await _context.SaveChangesAsync();
		}
    }
}
