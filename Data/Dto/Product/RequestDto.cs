namespace InventoryManager.API.Data.Dto.Product
{
    public class RequestDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
    }
}
