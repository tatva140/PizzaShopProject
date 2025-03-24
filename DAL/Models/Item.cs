using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Item
{
    public int ItemId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Rate { get; set; }

    public int Quantity { get; set; }

    public string Unit { get; set; }

    public bool IsAvailable { get; set; }

    public string? Description { get; set; }

    public string? ItemImg { get; set; }

    public decimal? TaxPercentage { get; set; }
    public bool DefaultTax { get; set; }

    public int? CategoryId { get; set; }

    public bool IsActive { get; set; }

    public bool? Isfavourite { get; set; }

    public string? Shortcode { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public int? CreatedBy { get; set; }

    public string ItemType { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<ModifierItem> ModifierItems { get; } = new List<ModifierItem>();

    public virtual ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();
    public virtual ICollection<OrderModifier> OrderModifiers { get; set; } = new List<OrderModifier>();

    public virtual User? UpdatedByNavigation { get; set; }
}
