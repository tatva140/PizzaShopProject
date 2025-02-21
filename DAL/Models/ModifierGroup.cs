using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ModifierGroup
{
    public int ModifierGroupId { get; set; }

    public string? ModifierGroupName { get; set; }

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    public bool? IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Category? Category { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Modifier> Modifiers { get; } = new List<Modifier>();

    public virtual User? UpdatedByNavigation { get; set; }
}
