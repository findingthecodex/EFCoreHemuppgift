using System;
using System.Threading.Tasks;
using EFCoreHemuppgift;
using Microsoft.EntityFrameworkCore;
namespace EFCoreHemuppgift.Services;

public class CustomerService
{
    public static async Task CustomerListAsync()
    {
        using var db = new ShopContext();
        var customers = await db.Customers
            .AsNoTracking()
            .OrderBy(c => c.CustomerId)
            .ToListAsync();
        Console.WriteLine("Customers:");
        Console.WriteLine("ID | Name | City | Email");
        
        foreach (var customer in customers)
        {
            Console.WriteLine($"{customer.CustomerId} | {customer.CustomerName} | {customer.CustomerCity} | {customer.CustomerEmail}");
        }
    }

    public static async Task CustomerAddAsync()
    {
        
        Console.Write("Please enter the name of the customer: ");
        var customerName = Console.ReadLine()?.Trim() ?? string.Empty;

        if (string.IsNullOrEmpty(customerName) || customerName.Length > 50)
        {
            Console.WriteLine("Customer Name is required, max 50.");
        }
        
        Console.Write("Please enter the city of the customer: ");
        var customerCity = Console.ReadLine();
        
        if (string.IsNullOrEmpty(customerCity) || customerCity.Length > 50)
        {
            Console.WriteLine("Customer City is required, max 50.");
        }
        
        Console.Write("Please enter the Email of the customer: ");
        var customerEmail = Console.ReadLine();
        
        if (string.IsNullOrEmpty(customerEmail) || customerEmail.Length > 50)
        {
            Console.WriteLine("Customer Email is required, max 50.");
        }
        
        using var db = new ShopContext();
        db.Customers.Add(new Customer {CustomerName = customerName, CustomerCity = customerCity, CustomerEmail = customerEmail});
        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Customer added successfully.");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error adding customer: {ex.InnerException?.Message ?? ex.Message}");
        }
    }

    public static async Task CustomerEditAsync(int customerId)
    {
        using var db = new ShopContext();
        var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerId == 1);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }
         
        Console.Write($"{customer.CustomerName}");
        var customername = Console.ReadLine()?.Trim()?? string.Empty;
        if (string.IsNullOrEmpty(customername))
        {
            customername = customer.CustomerName;
        }
        
        Console.Write($"{customer.CustomerEmail}");
        var customercity = Console.ReadLine()?.Trim()?? string.Empty;
        if (string.IsNullOrEmpty(customercity))
        {
            customercity = customer.CustomerEmail;
        }
        
        Console.Write($"{customer.CustomerCity}");
        var customeremail = Console.ReadLine()?.Trim()?? string.Empty;
        if (string.IsNullOrEmpty(customeremail))
        {
            customeremail = customer.CustomerEmail;
        }

        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Customer updated successfully.");
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            throw;
        }
    }

    public static async Task CustomerDeleteAsync()
    {
        using var db = new ShopContext();
        
        var customer = await db.Customers.FirstOrDefaultAsync(c => c.CustomerId == 1);
        if (customer == null)
        {
            Console.WriteLine("Customer not found.");
            return;
        }
        db.Customers.Remove(customer);
        try
        {
            await db.SaveChangesAsync();
            Console.WriteLine("Customer deleted successfully.");
        }
        catch (DbUpdateException exeption)
        {
            Console.WriteLine(exeption.Message);
        }
    }
}


