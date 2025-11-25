using System;
using System.Threading.Tasks;
using EFCoreHemuppgift;
using Microsoft.EntityFrameworkCore;
namespace EFCoreHemuppgift.Services;

public class OrderService
{
    public static async Task OrderListAsync()
    {
        using var db = new ShopContext();
        var orders = await db.Orders
            .AsNoTracking()
            .OrderBy(c => c.OrderId).Include(order => order.Customer)
            .ToListAsync();
        Console.WriteLine("Orders:");
        Console.WriteLine("OrderID | Name | OrderDate | TotalAmount | OrderStatus");
        
        foreach (var order in orders)
        {
            Console.WriteLine($"{order.OrderId} | {order.Customer?.CustomerName} | {order.OrderDate} | {order.TotalAmount} | {order.OrderStatus}");
        }
    }
    public static async Task OrderDetailsAsync(int detailsId)
    {
        using var db = new ShopContext();

        var orderdetails = await db.Orders
            .AsNoTracking()
            .OrderBy(x => x.OrderId)
            .Include(o => o.OrderRows)
            .ThenInclude(x => x.Product)
            .ToListAsync();
        Console.WriteLine("Order Details:");
        Console.WriteLine("OrderID | ProductName | Quantity | Price per unit | Row Total | Order Total");
        foreach (var order in orderdetails)
        {
            if (order.OrderId == detailsId)
            {
                foreach (var orderRow in order.OrderRows!)
                {
                    var rowTotal = orderRow.OrderQuantity * orderRow.UnitPrice;
                    var orderTotal = orderRow.OrderQuantity * rowTotal;
                    Console.WriteLine($"{order.OrderId} | {orderRow.Product?.ProductName} | {orderRow.OrderQuantity} | {orderRow.UnitPrice} | {rowTotal}");
                    Console.WriteLine($"Total Amount: {orderTotal}");
                }
            }
        }
    }

    public static async Task OrderAddAsync()
    {
        await CustomerService.CustomerListAsync();
        using var db = new ShopContext();
        Console.Write("Please enter the Customer ID for the order: ");

        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine("Customer ID is required.");
            return;
        }
       
        Console.Write("Please enter order rows")
    }
}

