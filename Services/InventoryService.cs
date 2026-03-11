using System;
using System.Collections.Generic;
using InventorySystem.Data;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    public class InventoryService
    {
         private readonly AppDbContext _context = new AppDbContext();
        

        public void CheckStock()
{
    Console.Write("Enter Product Id to search: ");
    int id = Convert.ToInt32(Console.ReadLine());

    var product = _context.Products.FirstOrDefault(p => p.Id == id);

    if (product != null)
    {
        Console.WriteLine("\nProduct Found");
        Console.WriteLine($"Id: {product.Id}");
        Console.WriteLine($"Name: {product.Name}");
        Console.WriteLine($"Quantity: {product.Quantity}");
        Console.WriteLine($"Price: {product.Price}");
    }
    else
    {
        Console.WriteLine("Product not found.");
    }
}

        public void AddProduct()
        
    {
        Console.Write("Enter Product Name: ");
        string? name = Console.ReadLine();

        if (string.IsNullOrEmpty(name))
    {
        Console.WriteLine("Product name cannot be empty.");
        return;
    }


        Console.Write("Enter Quantity: ");
        int quantity = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter Price: ");
        double price = Convert.ToDouble(Console.ReadLine());

        Product product = new Product
        {
            Name = name,
            Quantity = quantity,
            Price = price
        };

        _context.Products.Add(product);
        _context.SaveChanges();

        Console.WriteLine($"Product added with ID {product.Id}");
    }
        public void ViewProducts()
{
    var products = _context.Products.ToList();

    Console.WriteLine("\nProduct List:");
    Console.WriteLine("----------------------");

    foreach (var product in products)
    {
        Console.WriteLine($"Id: {product.Id}");
        Console.WriteLine($"Name: {product.Name}");
        Console.WriteLine($"Quantity: {product.Quantity}");
        Console.WriteLine($"Price: {product.Price}");
        Console.WriteLine("----------------------");
    }
}
       public void UpdateProduct()
{
    Console.Write("Enter Product Id to Update: ");
    int id = Convert.ToInt32(Console.ReadLine());

    var product = _context.Products.FirstOrDefault(p => p.Id == id);

    if (product != null)
    {
        Console.Write("Enter new product name: ");
        string? newName = Console.ReadLine();

        if (!string.IsNullOrEmpty(newName))
        {
            product.Name = newName;
        }

        Console.Write("Enter new quantity: ");
        product.Quantity = Convert.ToInt32(Console.ReadLine());

        Console.Write("Enter new price: ");
        product.Price = Convert.ToDouble(Console.ReadLine());

        _context.SaveChanges();

        Console.WriteLine("Product updated successfully!");
    }
    else
    {
        Console.WriteLine("Product not found.");
    }
}

        public void DeleteProduct()
{
    Console.Write("Enter Product Id to delete: ");
    int id = Convert.ToInt32(Console.ReadLine());

    var product = _context.Products.FirstOrDefault(p => p.Id == id);

    if (product != null)
    {
        _context.Products.Remove(product);
        _context.SaveChanges();

        Console.WriteLine("Product deleted successfully!");
    }
    else
    {
        Console.WriteLine("Product not found.");
    }
}
        public void LowStockAlert()
{
    var lowStockProducts = _context.Products.Where(p => p.Quantity < 5).ToList();

    bool found = lowStockProducts.Count > 0;

    Console.WriteLine("\nLow Stock Products (Quantity < 5)");
    Console.WriteLine("----------------------------------");

    foreach (var product in lowStockProducts)
    {
        Console.WriteLine($"Id: {product.Id}");
        Console.WriteLine($"Name: {product.Name}");
        Console.WriteLine($"Quantity: {product.Quantity}");
        Console.WriteLine($"Price: {product.Price}");
        Console.WriteLine("-----------------------------");
    }

    if (!found)
    {
        Console.WriteLine("No low stock products.");
    }
}
    }
}