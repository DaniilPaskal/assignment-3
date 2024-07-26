using System.ComponentModel.DataAnnotations.Schema;

namespace assignment_3.Models;

public class Cart
{
    public int CartId { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }

    public virtual List<Product> Products { get; set; }

    public virtual List<int> Quantities { get; set; }
}