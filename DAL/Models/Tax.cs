using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Tax
{
    public int TaxId { get; set; }

    public string? TaxName { get; set; }

    public string Amount { get; set; }

    public string? Type { get; set; }

    public bool? IsActive { get; set; }

    public bool IsEnabled { get; set; }

    public bool? IsDefault { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<OrderTax> OrderTaxes { get; } = new List<OrderTax>();

    public virtual User? UpdatedByNavigation { get; set; }
}
