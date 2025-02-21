using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Section
{
    public int SectionId { get; set; }

    public string? SectionName { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public int? CreatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Table> Tables { get; } = new List<Table>();

    public virtual User? UpdatedByNavigation { get; set; }
}
