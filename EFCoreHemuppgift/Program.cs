using System;
using EFCoreHemuppgift;
using EFCoreHemuppgift.Models;
using Microsoft.EntityFrameworkCore;


Console.WriteLine("Db: Path.Combine(AppContext.BaseDirectory, \"Shop.db\")");
using (var db = new ShopContext())
{
   await db.Database.MigrateAsync();

   if (!await db.Customers.AnyAsync())
   {
      db.Customers.AddRange(
         new Customer { CustomerId = 1, CustomerName = "Amanda Edenå", CustomerEmail = "amanda.edenå@example.com", CustomerCity = "Stockholm" },
         new Customer { CustomerId = 2, CustomerName = "Björn Svensson", CustomerEmail = "bjorn.svensson@example.com", CustomerCity = "Göteborg" },
         new Customer { CustomerId = 3, CustomerName = "Carina Larsson", CustomerEmail = "carina.larsson@example.com", CustomerCity = "Malmö" }
      );
      await db.SaveChangesAsync();
      Console.WriteLine("Seeded Customers");
   }

   if (!await db.Orders.AnyAsync())
   {
      db.Orders.AddRange(
         new Order { OrderId = 1, CustomerId = 1, OrderDate = DateTime.Now.AddDays(-10), OrderStatus = "Shipped"},
         new Order { OrderId = 2, CustomerId = 2, OrderDate = DateTime.Now.AddDays(-5), OrderStatus = "Processing"}
         );
      await db.SaveChangesAsync();
      Console.WriteLine("Seeded Orders");
   }
   
   if(!await db.Products.AnyAsync())
   {
      db.Products.AddRange(
         new Product { ProductId = 1, ProductName = "Laptop", ProductPrice = 10000.00m, ProductDescription = "MacBook"},
         new Product { ProductId = 2, ProductName = "Smartphone", ProductPrice = 5000.00m, ProductDescription = "iPhone"},
         new Product { ProductId = 3, ProductName = "Tablet", ProductPrice = 3000.00m, ProductDescription = "iPad"}
      );
      await db.SaveChangesAsync();
      Console.WriteLine("Seeded Products");
   }
   
   if(!await db.OrderRows.AnyAsync())
   {
      db.OrderRows.AddRange(
         new OrderRow { OrderRowId = 1, OrderId = 1, ProductId = 1, OrderQuantity = 1, UnitPrice = 10000.00m },
         new OrderRow { OrderRowId = 2, OrderId = 1, ProductId = 2, OrderQuantity = 1, UnitPrice = 5000.00m },
         new OrderRow { OrderRowId = 3, OrderId = 2, ProductId = 3, OrderQuantity = 1, UnitPrice = 3000.00m }
      );
      await db.SaveChangesAsync();
      Console.WriteLine("Seeded OrderRows");
   }
   
   // CLI CRUD
   while (true)
   {
      Console.WriteLine("\nChoose an option:");
      Console.WriteLine("1. Customers");
      Console.WriteLine("2. Orders");
      Console.WriteLine("Exit- Shutdown");
      Console.WriteLine(" ");
      
      var choice = Console.ReadLine();
      if (choice == "1")
         await CustomerMenu();
      else if (choice == "2")
         await OrderMenu();
      else if (choice == "3")
         break;
      else
      {
         Console.WriteLine("Invalid choice");
      }
   }
}
static async Task CustomerMenu()
{
   while (true)
   {
      Console.WriteLine("\nCustomer Menu: 1. List | 2. Add | 3. Edit (3 <id>) | 4. Delete | 5. Exit");
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
         case"1":
            await CustomerService.CustomerListAsync();
            break;
         case"2":
            await CustomerService.CustomerAddAsync();
            break;
         case"3":
            await CustomerService.CustomerListAsync();
            
            if (parts.Length < 2 || !int.TryParse(parts[1], out int editId))
            {
               Console.WriteLine("Please provide a valid Customer ID to view details.");
               break;
            }
            await CustomerService.CustomerEditAsync(editId);
            break;
         
         case"4":
            await CustomerService.CustomerDeleteAsync();
            break;
         case"5":
            return;
         default:
            Console.WriteLine("Unknown command");
            break;
      }
   }
}

static async Task OrderMenu()
{
   while (true)
   {
      Console.WriteLine("\nOrder Menu: 1. Order-List | 2. Order-Details | 3. Add-Order | 4. Exit");
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
         case"1":
            await OrderService.OrderListAsync();
            break; 
         case"2":
            
            if (parts.Length < 2 || !int.TryParse(parts[1], out int detailsId))
            {
               Console.WriteLine("Please provide a valid Customer ID to view details.");
               break;
            }
            await OrderService.OrderDetailsAsync(detailsId);
            break;
         case"3":
            await OrderService.OrderAddAsync();
            break;
         case"4":
            return;
         default:
            Console.WriteLine("Unknown command");
            break;
      }
   }
}