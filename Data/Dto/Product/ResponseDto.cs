namespace InventoryManager.API.Data.Dto.Product
{
    public class ResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
    }
}
