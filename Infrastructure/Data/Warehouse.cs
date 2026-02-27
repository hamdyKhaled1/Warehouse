using System;
using System.Collections.Generic;

namespace Warehouse.Infrastructure.Data;

public partial class Warehouse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Location { get; set; }

    public bool IsDeleted { get; set; } = false;

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
