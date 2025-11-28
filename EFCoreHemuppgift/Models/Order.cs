using System.ComponentModel.DataAnnotations;

namespace EFCoreHemuppgift.Models;

public class Order
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    [Required]
    public DateTime OrderDate { get; set; }
    
    [Required] 
    public string OrderStatus { get; set; } = null!;
    
    [Required]
    public decimal TotalAmount { get; set; }
    
    public Customer? Customer { get; set; }

    public List<OrderRow> OrderRows { get; set; } = new();
}