using Warehouse.Features.Orders.CreateOrder;

namespace Warehouse.Features.Orders.GetOrder.DTO
{
    public class OrderDto
    {

        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public decimal? TotalAmount { get; set; }
        public string Status { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
