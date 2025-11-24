namespace EFCoreHemuppgift.Models;

public class Product
{
    // PK
    public int ProductId { get; set; }
    
    // Properties
    public string? ProductName { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
}