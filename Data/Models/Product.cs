using System;

namespace InventoryManager.API.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        
        public virtual Category Category { get; set; }
        public Product()
        {
            this.CreatedDate = DateTime.Now;
            this.ModifiedDate = DateTime.Now;
        }
    }
}
