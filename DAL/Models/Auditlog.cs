using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Auditlog
{
    public int AuditId { get; set; }

    public int? UserId { get; set; }

    public DateTime? Login { get; set; }

    public DateTime? Logout { get; set; }

    public DateTime? Updated { get; set; }

    public DateTime? Deleted { get; set; }

    public virtual User? User { get; set; }
}
