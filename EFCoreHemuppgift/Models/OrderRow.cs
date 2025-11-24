namespace EFCoreHemuppgift.Models;

public class OrderRow
{
    // PK
    public int OrderRowId { get; set; }
    
    // FK
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    
    // Properties
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    
    // Navigation
    public Order? Order { get; set; }
    public Product? Product { get; set; }
}