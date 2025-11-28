namespace EFCoreHemuppgift.Models;

public class Product
{
    // PK
    public int ProductId { get; set; }
    
    // Properties
    public string? ProductName { get; set; }
    public decimal ProductPrice { get; set; }
    public string? ProductDescription { get; set; }
}