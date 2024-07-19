using System.ComponentModel.DataAnnotations.Schema;

namespace assignment_3.Models;

public class Comment
{
    public int CommentId { get; set; }
    public double Rating { get; set; }
    public virtual ICollection<string> Images { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public Product Product { get; set; }
}