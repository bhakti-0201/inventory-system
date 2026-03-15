using System;
using InventorySystem.Services;

class Program
{
    static void Main()
    {
        InventoryService? service = null;
        
        try
        {
            service = new InventoryService();
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Database Connection Error: {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error initializing service: {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            return;
        }
        
        bool running = true;

        while (running)
        {
            try
            {
                Console.WriteLine("\nInventory Management System");
                Console.WriteLine("1. Add Product");
                Console.WriteLine("2. View Products");
                Console.WriteLine("3. Update Product");
                Console.WriteLine("4. Delete Product");
                Console.WriteLine("5. Check Stock");
                Console.WriteLine("6. Low Stock Alert");
                Console.WriteLine("7. Exit");

                Console.Write("Choose an option: ");
                string? input = Console.ReadLine();

                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("Invalid input. Please enter a number between 1 and 7.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        service.AddProduct();
                        break;

                    case 2:
                        service.ViewProducts();
                        break;

                    case 3:
                        service.UpdateProduct();
                        break;

                    case 4:
                        service.DeleteProduct();
                        break;

                    case 5:
                        service.CheckStock();
                        break;

                    case 6:
                        service.LowStockAlert();
                        break;

                    case 7:
                        running = false;
                        Console.WriteLine("Exiting program...");
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please enter a number between 1 and 7.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
                Console.WriteLine("Please try again or contact support if the problem persists.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
        }
    }
}