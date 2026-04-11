using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventorySystem.Data;
using InventorySystem.Models;
using InventorySystem.Repositories;

namespace InventorySystem.Services
{
    public class InventoryService
    {
        private readonly IProductRepository _repository;
        private readonly AppDbContext _context;

        public InventoryService()
        {
            try
            {
                _context = new AppDbContext();
                _context.Database.CanConnect();
                _repository = new ProductRepository(_context);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to connect to database. Please check your connection string and ensure PostgreSQL is running.", ex);
            }
        }

        // Constructor for dependency injection
        public InventoryService(IProductRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _context = new AppDbContext();
        }

        public async Task CheckStockAsync()
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

                var product = await _repository.GetByIdAsync(id);

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
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking stock: {ex.Message}");
            }
        }

        // Sync version for backward compatibility
        public void CheckStock()
        {
            CheckStockAsync().GetAwaiter().GetResult();
        }

        public async Task AddProductAsync()
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

                var addedProduct = await _repository.AddAsync(product);
                Console.WriteLine($"Product added with ID {addedProduct.Id}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
            }
        }

        // Sync version for backward compatibility
        public void AddProduct()
        {
            AddProductAsync().GetAwaiter().GetResult();
        }

        public async Task ViewProductsAsync()
        {
            try
            {
                var products = await _repository.GetAllAsync();

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
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving products: {ex.Message}");
            }
        }

        // Sync version for backward compatibility
        public void ViewProducts()
        {
            ViewProductsAsync().GetAwaiter().GetResult();
        }

        public async Task UpdateProductAsync()
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

                var product = await _repository.GetByIdAsync(id);

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

                await _repository.UpdateAsync(product);
                Console.WriteLine("Product updated successfully!");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating product: {ex.Message}");
            }
        }

        // Sync version for backward compatibility
        public void UpdateProduct()
        {
            UpdateProductAsync().GetAwaiter().GetResult();
        }

        public async Task DeleteProductAsync()
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

                var product = await _repository.GetByIdAsync(id);

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

                bool deleted = await _repository.DeleteAsync(id);
                if (deleted)
                {
                    Console.WriteLine("Product deleted successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to delete product.");
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting product: {ex.Message}");
            }
        }

        // Sync version for backward compatibility
        public void DeleteProduct()
        {
            DeleteProductAsync().GetAwaiter().GetResult();
        }

        public async Task LowStockAlertAsync()
        {
            try
            {
                var lowStockProducts = await _repository.GetLowStockProductsAsync(5);

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
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking low stock: {ex.Message}");
            }
        }

        // Sync version for backward compatibility
        public void LowStockAlert()
        {
            LowStockAlertAsync().GetAwaiter().GetResult();
        }
    }
}