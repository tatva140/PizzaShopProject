using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Modifier
{
    public int ModifierId { get; set; }

    public string? ModifierName { get; set; }

    public string? Description { get; set; }

    public int? ModifierGroupId { get; set; }

    public decimal Rate { get; set; }

    public int? Quantity { get; set; }

    public decimal? Unit { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public int? CreatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ModifierGroup? ModifierGroup { get; set; }

    public virtual ICollection<ModifierItem> ModifierItems { get; } = new List<ModifierItem>();

    public virtual ICollection<OrderModifier> OrderModifiers { get; } = new List<OrderModifier>();

    public virtual User? UpdatedByNavigation { get; set; }
}
