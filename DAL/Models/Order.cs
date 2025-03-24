using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public decimal? TotalAmount { get; set; }
    public decimal? SubTotal { get; set; }

    public string? PaymentMode { get; set; }

    public string? Instructions { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public decimal? TotalTax { get; set; }

    public decimal? TotalDiscount { get; set; }

    public string? OrderStatus { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();

    public virtual ICollection<OrderTax> OrderTaxes { get; } = new List<OrderTax>();

    public virtual ICollection<Payment> Payments { get; } = new List<Payment>();
    public virtual ICollection<OrderModifier> OrderModifiers { get; set; } = new List<OrderModifier>();

    public virtual User? UpdatedByNavigation { get; set; }
}
