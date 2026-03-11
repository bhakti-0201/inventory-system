using System;
using InventorySystem.Services;

class Program
{
    static void Main()
    {
        InventoryService service = new InventoryService();
        bool running = true;

        while (running)
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
            int choice;

             if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
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
                    Console.WriteLine("Invalid option. Try again.");
                    break;
            }
        }
    }
}