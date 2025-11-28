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
        Console.WriteLine("OrderID | Name | Product | OrderDate | TotalAmount | OrderStatus");

        foreach (var order in orders)
        {
            Console.WriteLine(
                $"{order.OrderId} | {order.Customer?.CustomerName} | {order.OrderDate} | {order.TotalAmount} | {order.OrderStatus}");
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
                    Console.WriteLine(
                        $"{order.OrderId} | {orderRow.Product?.ProductName} | {orderRow.OrderQuantity} | {orderRow.UnitPrice} | {rowTotal}");
                    Console.WriteLine($"Total Amount: {orderTotal}");
                }
            }
        }
    }

    public static async Task OrderAddAsync()
    {
        await CustomerService.CustomerListAsync();
        using var db = new ShopContext();
        Console.WriteLine(" ");
        Console.Write("Please enter the Customer ID for the new order: ");

        if (!int.TryParse(Console.ReadLine(), out int customerId))
        {
            Console.WriteLine("Customer ID is required.");
            return;
        }

        var customer = await db.Customers.FindAsync(customerId);
        if (customer == null)
        {
            Console.WriteLine("Customer ID is required.");
        }

        Console.WriteLine("Products available: ");
        var products = await db.Products
            .AsNoTracking()
            .OrderBy(p => p.ProductId)
            .ToListAsync();
        foreach (var product in products)
        {
            Console.WriteLine(
                $"{product.ProductId} | {product.ProductName} | {product.ProductPrice} | {product.ProductDescription}");
        }

        var orderRows = new List<OrderRow>();

        while (true)
        {
            Console.WriteLine(" ");
            Console.WriteLine("Add product to the order (ProductID): ");
            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Product ID is required.");
                return;
            }

            var productToAdd = await db.Products.FindAsync(productId);
            if (productToAdd == null)
            {
                Console.WriteLine("Product ID is required.");
            }

            Console.WriteLine("Product: " + productToAdd?.ProductName + " | Price: " + productToAdd?.ProductPrice);

            Console.WriteLine(" ");
            Console.Write("Enter quantity: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
            {
                Console.WriteLine("Quantity must be a positive integer.");
                continue;
            }

            var row = new OrderRow
            {
                ProductId = productToAdd.ProductId,
                OrderQuantity = quantity,
                UnitPrice = productToAdd.ProductPrice
            };
            orderRows.Add(row);

            Console.WriteLine(" ");
            Console.WriteLine("Product(s) added to order: " + productToAdd?.ProductName + " | Quantity: " + quantity +
                              " | Unit Price: " + productToAdd?.ProductPrice);

            Console.WriteLine("Do you want to add more products? (y/n): ");
            var addMore = Console.ReadLine()?.Trim().ToLower();
            if (addMore == "y")
            {
                continue;
            }

            break;
        }

        if (!orderRows.Any())
        {
            Console.WriteLine("No products added to the order. Order creation cancelled.");
            return;
        }

        decimal total = orderRows.Sum(x => x.UnitPrice * x.OrderQuantity);

        var newOrder = new Order
        {
            CustomerId = customerId,
            OrderDate = DateTime.Now,
            OrderStatus = "Pending",
            TotalAmount = total,
            OrderRows = orderRows
        };

        db.Orders.Add(newOrder);
        await db.SaveChangesAsync();

        Console.WriteLine("Order Summary:");
        foreach (var x in orderRows)
        {
            Console.WriteLine("ProductID: " + x.ProductId + " | + Product(s): " + x.Product + " | Quantity: " +
                              x.OrderQuantity + " | Unit Price: " + x.UnitPrice);
        }

        Console.WriteLine($"\nTOTAL ORDER SUM: {total}");
        Console.WriteLine($"Order saved with OrderId: {newOrder.OrderId}");
    }

    public static async Task OrderByStatusAsync()
    {
        using var db = new ShopContext();

        var orders = await db.Orders
            .Include(o => o.Customer)
            .OrderBy(o => o.OrderDate)
            .ToListAsync();

        Console.WriteLine("All orders: ");
        Console.WriteLine("OrderID | Customer | OrderDate | TotalAmount | OrderStatus");
        foreach (var order in orders)
        {
            Console.WriteLine(
                $"OrderID: {order.OrderId} | Customer: {order.Customer?.CustomerName} | OrderDate: {order.OrderDate} | TotalAmount: {order.TotalAmount} | OrderStatus: {order.OrderStatus}");
        }

        Console.WriteLine();
        Console.WriteLine("Enter order status (Pending, Processing, Paid, Shipped, Delivered): ");
        var statusInput = Console.ReadLine()?.Trim().ToLowerInvariant();
        if (string.IsNullOrEmpty(statusInput))
        {
            Console.WriteLine("Order status is required.");
            return;
        }

        var filteredOrders = orders
            .Where(o => o.OrderStatus.Equals(statusInput, StringComparison.OrdinalIgnoreCase))
            .ToList();
        if (!filteredOrders.Any())
        {
            Console.WriteLine($"No orders found with status '{statusInput}'.");
            return;
        }

        Console.WriteLine($"\nOrders with status '{statusInput}': ");
        Console.WriteLine("OrderID | Customer | OrderDate | TotalAmount | OrderStatus");
        foreach (var order in filteredOrders)
        {
            Console.WriteLine(
                $"OrderID: {order.OrderId} | Customer: {order.Customer?.CustomerName} | OrderDate: {order.OrderDate} | TotalAmount: {order.TotalAmount} | OrderStatus: {order.OrderStatus}");
        }
    }

    public static async Task OrdersByCustomers()
    {
        using var db = new ShopContext();

        var customers = await db.Customers
            .Include(c => c.Orders)!
            .ThenInclude(o => o.OrderRows)
            .ThenInclude(or => or.Product)
            .OrderBy(c => c.CustomerId)
            .ToListAsync();

        Console.WriteLine("Orders by Customers:");
        foreach (var customer in customers)
        {
            Console.WriteLine($"\nCustomer: {customer.CustomerName} (ID: {customer.CustomerId})");
            if (customer.Orders == null || !customer.Orders.Any())
            {
                Console.WriteLine("  No orders found.");
                continue;
            }

            foreach (var order in customer.Orders)
            {
                Console.WriteLine(
                    $"  Order ID: {order.OrderId} | Order Date: {order.OrderDate} | Total Amount: {order.TotalAmount} | Status: {order.OrderStatus}");
                if (!order.OrderRows.Any())
                {
                    Console.WriteLine("    No order rows found.");
                    continue;
                }

                foreach (var orderRow in order.OrderRows)
                {
                    Console.WriteLine(
                        $"    Product: {orderRow.Product?.ProductName} | Quantity: {orderRow.OrderQuantity} | Unit Price: {orderRow.UnitPrice}");
                }
            }
        }
    }
    
    public static async Task OrdersPage(int page, int pageSize)
    {
        using var db = new ShopContext();
        int currentPage = Math.Max(page, 1);
    
        while (true)
        {
            var query = db.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderRows)
                .ThenInclude(or => or.Product)
                .OrderByDescending(o => o.OrderDate)
                .AsNoTracking();
    
            var totalCount = await query.CountAsync();
            var totalPages = Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize));
    
            if (currentPage > totalPages) currentPage = totalPages;
            if (currentPage < 1) currentPage = 1;
    
            var pageItems = await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
    
            Console.WriteLine($"\nPage {currentPage}/{totalPages} (pageSize={pageSize}, total={totalCount})");
            Console.WriteLine("CustomerName | CustomerID | OrderDate | TotalAmount | OrderStatus");
            foreach (var order in pageItems)
            {
                Console.WriteLine($"{order.Customer?.CustomerName} | {order.Customer?.CustomerId} | {order.OrderDate} | {order.TotalAmount} | {order.OrderStatus}");
            }
    
            Console.WriteLine("\nCommands: n = next, p = previous, q = quit");
            Console.Write("Enter command: ");
            var cmd = (Console.ReadLine() ?? string.Empty).Trim().ToLowerInvariant();
    
            if (cmd == "q") break;
            if (cmd == "n")
            {
                if (currentPage < totalPages) currentPage++;
                else Console.WriteLine("Already on last page.");
                continue;
            }
            if (cmd == "p")
            {
                if (currentPage > 1) currentPage--;
                else Console.WriteLine("Already on first page.");
                continue;
            }
    
            Console.WriteLine("Unknown command.");
        }
    }
    

    public static async Task StatusMenu()
    {
        while (true)
        {
            Console.WriteLine(
                "\nStatus: 1. Order-By-Status | 2. Order-By-Customers | 3. Orders-Page | 4. Exit");
            Console.WriteLine(">");
            var line = Console.ReadLine()?.Trim() ?? string.Empty;

            if (line.Equals("..", StringComparison.OrdinalIgnoreCase))
            {
                break;
            }

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var cmd = parts[0].ToLowerInvariant();

            switch (cmd)
            {
                case "1":
                    await OrderByStatusAsync();
                    break;
                case "2":
                    await OrdersByCustomers();
                    break;
                case "3":
                    await OrdersPage(1, 10); 
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Unknown command");
                    break;
            }
        }
    }
}

        