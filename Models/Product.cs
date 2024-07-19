namespace assignment_3.Models;

public class Product
{
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public double Cost { get; set; }
    public double ShippingCost { get; set; }
    public double Rating { get; set; }
}