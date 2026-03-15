using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using InventorySystem.Data;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    public class InventoryService
    {
        private readonly AppDbContext _context = new AppDbContext();

        public InventoryService()
        {
            try
            {
                _context.Database.CanConnect();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to connect to database. Please check your connection string and ensure PostgreSQL is running.", ex);
            }
        }

        public void CheckStock()
        {
            try
            {
                Console.Write("Enter Product Id to search: ");
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int id))
                {
                    Console.WriteLine("Invalid product ID. Please enter a valid number.");
                    return;
                }

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
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format. Please enter a valid number.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking stock: {ex.Message}");
            }
        }

        public void AddProduct()
        {
            try
            {
                Console.Write("Enter Product Name: ");
                string? name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Product name cannot be empty.");
                    return;
                }

                Console.Write("Enter Quantity: ");
                string? quantityInput = Console.ReadLine();
                if (!int.TryParse(quantityInput, out int quantity) || quantity < 0)
                {
                    Console.WriteLine("Invalid quantity. Please enter a non-negative number.");
                    return;
                }

                Console.Write("Enter Price: ");
                string? priceInput = Console.ReadLine();
                if (!double.TryParse(priceInput, out double price) || price < 0)
                {
                    Console.WriteLine("Invalid price. Please enter a non-negative number.");
                    return;
                }

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
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database error while adding product: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
            }
        }

        public void ViewProducts()
        {
            try
            {
                var products = _context.Products.ToList();

                if (!products.Any())
                {
                    Console.WriteLine("No products found in the inventory.");
                    return;
                }

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving products: {ex.Message}");
            }
        }

        public void UpdateProduct()
        {
            try
            {
                Console.Write("Enter Product Id to Update: ");
                string? idInput = Console.ReadLine();

                if (!int.TryParse(idInput, out int id))
                {
                    Console.WriteLine("Invalid product ID. Please enter a valid number.");
                    return;
                }

                var product = _context.Products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                {
                    Console.WriteLine("Product not found.");
                    return;
                }

                Console.Write($"Enter new product name (current: {product.Name}): ");
                string? newName = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(newName))
                {
                    product.Name = newName;
                }

                Console.Write($"Enter new quantity (current: {product.Quantity}): ");
                string? quantityInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(quantityInput) && int.TryParse(quantityInput, out int newQuantity) && newQuantity >= 0)
                {
                    product.Quantity = newQuantity;
                }
                else if (!string.IsNullOrWhiteSpace(quantityInput))
                {
                    Console.WriteLine("Invalid quantity. Keeping current value.");
                }

                Console.Write($"Enter new price (current: {product.Price}): ");
                string? priceInput = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(priceInput) && double.TryParse(priceInput, out double newPrice) && newPrice >= 0)
                {
                    product.Price = newPrice;
                }
                else if (!string.IsNullOrWhiteSpace(priceInput))
                {
                    Console.WriteLine("Invalid price. Keeping current value.");
                }

                _context.SaveChanges();
                Console.WriteLine("Product updated successfully!");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database error while updating product: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
            }
        }

        public void DeleteProduct()
        {
            try
            {
                Console.Write("Enter Product Id to delete: ");
                string? idInput = Console.ReadLine();

                if (!int.TryParse(idInput, out int id))
                {
                    Console.WriteLine("Invalid product ID. Please enter a valid number.");
                    return;
                }

                var product = _context.Products.FirstOrDefault(p => p.Id == id);

                if (product == null)
                {
                    Console.WriteLine("Product not found.");
                    return;
                }

                Console.WriteLine($"Are you sure you want to delete '{product.Name}'? (y/n)");
                string? confirmation = Console.ReadLine();

                if (confirmation?.ToLower() != "y")
                {
                    Console.WriteLine("Deletion cancelled.");
                    return;
                }

                _context.Products.Remove(product);
                _context.SaveChanges();

                Console.WriteLine("Product deleted successfully!");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"Database error while deleting product: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
            }
        }

        public void LowStockAlert()
        {
            try
            {
                var lowStockProducts = _context.Products.Where(p => p.Quantity < 5).ToList();

                Console.WriteLine("\nLow Stock Products (Quantity < 5)");
                Console.WriteLine("----------------------------------");

                if (!lowStockProducts.Any())
                {
                    Console.WriteLine("No low stock products.");
                    return;
                }

                foreach (var product in lowStockProducts)
                {
                    Console.WriteLine($"Id: {product.Id}");
                    Console.WriteLine($"Name: {product.Name}");
                    Console.WriteLine($"Quantity: {product.Quantity}");
                    Console.WriteLine($"Price: {product.Price}");
                    Console.WriteLine("-----------------------------");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking low stock: {ex.Message}");
            }
        }
    }
}