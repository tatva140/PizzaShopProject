using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Table
{
    public int TableId { get; set; }

    public string? TableName { get; set; }

    public int? SectionId { get; set; }
    public int? CurrentOrderId { get; set; }
    public int? CurrentCustomerId { get; set; }

    public int? Capacity { get; set; }

    public bool? IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? TableStatus { get; set; }

    public virtual ICollection<AllocatedTable> AllocatedTables { get; } = new List<AllocatedTable>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Section? Section { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
