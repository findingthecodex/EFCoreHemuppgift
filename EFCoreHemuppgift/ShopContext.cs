using EFCoreHemuppgift.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreHemuppgift;

public class ShopContext : DbContext
{
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderRow> OrderRows => Set<OrderRow>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(AppContext.BaseDirectory, "Shop.db");
        optionsBuilder.UseSqlite($"Filename={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(c =>
        {
            c.HasKey(x => x.CustomerId);
            
            c.Property(x => x.CustomerName).IsRequired().HasMaxLength(50);
            c.Property(x => x.CustomerId).IsRequired().HasMaxLength(50);
            c.Property(x => x.CustomerCity).HasMaxLength(50);
            c.Property(x => x.CustomerEmail).IsRequired().HasMaxLength(50);
            
            c.HasIndex(x => x.CustomerEmail).IsUnique();
        });

        modelBuilder.Entity<Order>(o =>
        {
            o.HasKey(x => x.OrderId);
            
            o.Property(x => x.OrderDate).IsRequired();
            o.Property(x => x.OrderStatus).IsRequired().HasMaxLength(20);
            o.Property(x => x.TotalAmount).IsRequired();
            
            o.HasOne(x => x.Customer)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<OrderRow>(o =>
        {
            o.HasKey(x => x.OrderRowId);
            
            o.Property(x => x.OrderQuantity).IsRequired();
            o.Property(x => x.UnitPrice).IsRequired();

            o.HasOne(x => x.Order)
                .WithMany(x => x.OrderRows)
                .HasForeignKey(x => x.OrderId)
                .OnDelete(DeleteBehavior.Restrict);
            o.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Product>(p =>
            {
                p.HasKey(x => x.ProductId);
                p.Property(x => x.ProductName).IsRequired().HasMaxLength(50);
                p.Property(x => x.ProductPrice).IsRequired();
                p.Property(x => x.ProductDescription).IsRequired();
            }
        );
    }
}