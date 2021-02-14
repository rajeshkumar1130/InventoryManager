using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryManager.API.Data.Interfaces;
using InventoryManager.API.Data.Models;
using System.Linq;

namespace InventoryManager.API.Data.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _appDbContext;

        public ProductService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task AddProduct(Product dto)
        {
            await _appDbContext.Products.AddAsync(dto);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<Product> GetProduct(int productId)
        {
            return await _appDbContext.Products.FirstOrDefaultAsync(x => x.IsDeleted != true && x.Id == productId);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _appDbContext.Products.Where(x => x.IsDeleted != true).ToListAsync();
        }


        public async Task<ActionResult> RemoveProduct(int productId)
        {
            var product = await _appDbContext.Products.FindAsync(productId);

            if(product == null)
            {
                return new NotFoundResult();
            }
            product.IsDeleted = true;
            await _appDbContext.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<bool> UpdateProduct(Product product, int productId)
        {
            Product productData = await _appDbContext.Products.FirstOrDefaultAsync(item => item.Id == productId);

            if (productData != null)
            {
                productData.Name = product.Name;
                productData.Description = product.Description;
                productData.Quantity = product.Quantity;
                productData.CategoryId = product.CategoryId;
                productData.Price = product.Price;
                await _appDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
