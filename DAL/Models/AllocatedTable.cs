using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class AllocatedTable
{
    public int AllocatedTableId { get; set; }

    public int? TableId { get; set; }

    public int? CustomerId { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Table? Table { get; set; }
}
