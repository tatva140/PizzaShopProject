using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class OrderItem
{
    public int OrderItemsId { get; set; }

    public int? OrderId { get; set; }

    public int? ItemId { get; set; }

    public int? Quantity { get; set; }

    public decimal? Rate { get; set; }

    public string? Instructions { get; set; }

    public int? ReadyQuantity { get; set; }

    public decimal? Tax { get; set; }

    public string? OrderStatus { get; set; }

    public virtual Item? Item { get; set; }

    public virtual Order? Order { get; set; }
}
