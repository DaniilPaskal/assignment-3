using System.ComponentModel.DataAnnotations.Schema;

namespace assignment_3.Models;

public class Comment
{
	public int Id { get; set; }
    public double Rating { get; set; }
    public string Text { get; set; }
    public virtual List<string> Images { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public Product Product { get; set; }
}