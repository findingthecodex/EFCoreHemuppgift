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
         new Customer { Name = "Amanda Edenå", Email = "amanda.edenå@example.com", City = "Stockholm" },
         new Customer { Name = "Björn Svensson", Email = "bjorn.svensson@example.com", City = "Göteborg" },
         new Customer { Name = "Carina Larsson", Email = "carina.larsson@example.com", City = "Malmö" }
      );
      await db.SaveChangesAsync();
      Console.WriteLine("Seeded Customers");
   }
   
   // CLI CRUD
   while (true)
   {
      Console.WriteLine("\nChoose an option:");
      Console.WriteLine("1. Customers");
      Console.WriteLine("2. Orders");
      Console.WriteLine("Exit- Shutdown");
      
      var choice = Console.ReadLine();
      if (choice == "1")
         await CustomerMenu();
      else if (choice == "2")
         await OrderMenu();
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
      Console.WriteLine("\nCustomer Menu: 1. List | 2. Add | 3. Update | 4. Delete | 5. Exit");
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
            await CustomerListAsync();
            break;
         case"2":
            await CustomerAddAsync();
            break;
         case"3":
            await CustomerUpdateAsync();
            break;
         case"4":
            await CustomerDeleteAsync();
            break;
         default:
            Console.WriteLine("Unknown command");
            break;
      }
   }
}