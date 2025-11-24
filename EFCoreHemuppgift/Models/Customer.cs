using System.ComponentModel.DataAnnotations;

namespace EFCoreHemuppgift.Models;

public class Customer
{
    public int CustomerId { get; set; }
    [Required, MaxLength(50)] 
    public string Name { get; set; } = null!;
    [Required, MaxLength(50)] 
    public string Email { get; set; } = null!;
    public string? City { get; set; }

    public List<Order>? Orders { get; set; } = new();
}