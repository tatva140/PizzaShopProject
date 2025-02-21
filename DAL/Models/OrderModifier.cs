using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class OrderModifier
{
    public int OrderModifierId { get; set; }

    public int? ModifierId { get; set; }

    public int? Quantity { get; set; }

    public decimal Rate { get; set; }

    public string? Instructions { get; set; }

    public virtual Modifier? Modifier { get; set; }
}
