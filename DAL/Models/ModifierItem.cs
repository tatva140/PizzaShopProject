using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ModifierItem
{
    public int ModifierItemsId { get; set; }

    public int? ModifierId { get; set; }

    public int? ItemId { get; set; }

    public bool? IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Item? Item { get; set; }

    public virtual Modifier? Modifier { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
