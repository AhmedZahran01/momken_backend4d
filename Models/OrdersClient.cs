using momken_backend.Enums;

namespace momken_backend.Models
{
    public class OrdersClient:ModelBaseId
    {
        public decimal OrderTotalPrice { get; set; }
        public List<Product> OrderproductsWithItsQuantity { get; set; }
        public OrderStatus orderStatus { get; set; } = OrderStatus.pending;

    }
}
