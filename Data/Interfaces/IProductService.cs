using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using InventoryManager.API.Data.Models;

namespace InventoryManager.API.Data.Interfaces
{
    public interface IProductService
    {
        Task AddProduct(Product dto);
        Task<Product> GetProduct(int productId);
        Task<List<Product>> GetProducts();
        Task<ActionResult> RemoveProduct(int productId);
        Task<bool> UpdateProduct(Product product, int productId);
    }
}
