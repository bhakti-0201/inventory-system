using System;
using System.Collections.Generic;
using InventorySystem.Models;

namespace InventorySystem.Services
{
    public class InventoryService
    {
        private List<Product> inventory =new List<Product>();
         private int nextId = 1;

        public void CheckStock()
        {
            Console.Write("Enter Product Id to search: ");
            int id = Convert.ToInt32(Console.ReadLine());

            foreach (var product in inventory)
            {
                if (product.Id == id)
                {
                    Console.WriteLine("\nProduct Found");
                    Console.WriteLine($"Id: {product.Id}");
                    Console.WriteLine($"Name: {product.Name}");
                    Console.WriteLine($"Quantity: {product.Quantity}");
                    Console.WriteLine($"Price: {product.Price}");
                    return;
                }
            }

            Console.WriteLine("Product not found.");
        }

        public void AddProduct()
        {
            Product product =new Product();

            product.Id = nextId++;

            Console.Write("Enter Product Name: ");
            product.Name = Console.ReadLine();

            Console.Write("Enter Quantity: ");
            product.Quantity = Convert.ToInt32(Console.ReadLine());

            Console.Write("Enter Price: ");
            product.Price = Convert.ToDouble(Console.ReadLine());
            

            inventory.Add(product);

            Console.WriteLine("Product added successfully!");
            Console.WriteLine($"Product ID: {product.Id}");
        }
        public void ViewProducts()
        {
            if (inventory.Count == 0)
            {
                Console.WriteLine("No products in inventory.");
                return;
            }

            Console.WriteLine("\nProduct List:");
            Console.WriteLine("---------------------------");

            foreach (var product in inventory)
            {
                Console.WriteLine($"Id: {product.Id}");
                Console.WriteLine($"Name: {product.Name}");
                Console.WriteLine($"Quantity: {product.Quantity}");
                Console.WriteLine($"Price: {product.Price}");
                Console.WriteLine("---------------------------");
            }
        }
        public void UpdateProduct()
        {
            Console.Write("Enter Product Id to Update: ");
            int id = Convert.ToInt32(Console.ReadLine());

            foreach (var product in inventory)
            {
                if (product.Id == id)
                {
                    Console.Write("Enter new product name: ");
                    product.Name =Console.ReadLine();

                    Console.Write("Enter new quantity: ");
                    product.Quantity = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Enter new price: ");
                    product.Price = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Product updated successfully!");
                    return;
                }
            }

            Console.WriteLine("Product not found.");
        }

        public void DeleteProduct()
        {
            Console.Write("Enter Product Id to delete: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Product productToDelete = null;

            foreach (var product in inventory)
            {
                if (product.Id == id)
                {
                    productToDelete = product;
                    break;
                }
            }

            if (productToDelete != null)
            {
                inventory.Remove(productToDelete);
                Console.WriteLine("Product deleted successfully!");
            }
            else
            {
                Console.WriteLine("Product not found.");
            }
        }

        public void LowStockAlert()
        {
            bool found = false;

            Console.WriteLine("\nLow Stock Products (Quantity < 5)");
            Console.WriteLine("----------------------------------");

            foreach (var product in inventory)
            {
                if (product.Quantity < 5)
                {
                    Console.WriteLine($"Id: {product.Id}");
                    Console.WriteLine($"Name: {product.Name}");
                    Console.WriteLine($"Quantity: {product.Quantity}");
                    Console.WriteLine($"Price: {product.Price}");
                    Console.WriteLine("-----------------------------");

                    found = true;
                }
            }

            if (!found)
            {
                Console.WriteLine("No low stock products.");
            }
        }
    }
}