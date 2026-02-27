using System;
using System.Collections.Generic;

namespace Warehouse.Infrastructure.Data;
public enum OrderStatus
{
    Pending,
    Processing,
    Completed,
    Cancelled
}
public partial class Order
{
    public int Id { get; set; }

    public string OrderNumber { get; set; } = null!;

    public decimal? TotalAmount { get; set; }

    public OrderStatus Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool IsDeleted { get; set; }= false;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
