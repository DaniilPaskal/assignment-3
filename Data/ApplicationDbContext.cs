using Microsoft.EntityFrameworkCore;
using assignment_3.Models;

namespace assignment_3.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<assignment_3.Models.User> User { get; set; } = default!;
        public DbSet<assignment_3.Models.Comment> Comment { get; set; } = default!;
        public DbSet<assignment_3.Models.Cart> Cart { get; set; } = default!;
        public DbSet<assignment_3.Models.Order> Order { get; set; } = default!;
    }
}