using System.ComponentModel.DataAnnotations;

namespace EFCoreHemuppgift.Models;

public class Customer
{
    public int CustomerId { get; set; }
    [Required, MaxLength(50)] 
    public string CustomerName { get; set; } = null!;
    [Required, MaxLength(50)] 
    public string? CustomerEmail { get; set; } = null!;
    public string? CustomerCity { get; set; }

    public List<Order>? Orders { get; set; } = new();
}

