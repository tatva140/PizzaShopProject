using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Permission
{
    public int PermissionId { get; set; }

    public bool? CanView { get; set; }

    public bool? CanAddEdit { get; set; }

    public bool? CanDelete { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public string? Role { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
