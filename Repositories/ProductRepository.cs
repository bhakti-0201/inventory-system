using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using InventorySystem.Data;
using InventorySystem.Models;

namespace InventorySystem.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving product with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                return await _context.Products.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving all products: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 5)
        {
            try
            {
                return await _context.Products
                    .Where(p => p.Quantity < threshold)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error retrieving low stock products: {ex.Message}", ex);
            }
        }

        public async Task<Product> AddAsync(Product product)
        {
            try
            {
                if (product == null)
                    throw new ArgumentNullException(nameof(product));

                var addedProduct = await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return addedProduct.Entity;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Database error while adding product: {ex.InnerException?.Message ?? ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error adding product: {ex.Message}", ex);
            }
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            try
            {
                if (product == null)
                    throw new ArgumentNullException(nameof(product));

                var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == product.Id);
                if (existingProduct == null)
                    throw new InvalidOperationException($"Product with ID {product.Id} not found.");

                _context.Entry(existingProduct).CurrentValues.SetValues(product);
                await _context.SaveChangesAsync();
                return existingProduct;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Database error while updating product: {ex.InnerException?.Message ?? ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error updating product: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
                if (product == null)
                    return false;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                throw new InvalidOperationException($"Database error while deleting product: {ex.InnerException?.Message ?? ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error deleting product: {ex.Message}", ex);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _context.Products.AnyAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error checking if product exists: {ex.Message}", ex);
            }
        }
    }
}
