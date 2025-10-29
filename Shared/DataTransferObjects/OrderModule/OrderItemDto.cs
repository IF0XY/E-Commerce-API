namespace Shared.DataTransferObjects.OrderModule
{
    public class OrderItemDto
    {
        public string ProductName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }
    }
}