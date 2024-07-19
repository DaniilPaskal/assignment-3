using System.ComponentModel.DataAnnotations.Schema;

namespace assignment_3.Models;

public class Order
{
    public int OrderId { get; set; }
    public DateTime Date { get; set; }
    public double Cost { get; set; }
    
    [ForeignKey("User")]
    public int UserId { get; set; }
    public User User { get; set; }
}