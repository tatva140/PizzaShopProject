using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int? CustomerId { get; set; }

    public int? OrderId { get; set; }

    public string? PaymentMode { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? PaymentStatus { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Order? Order { get; set; }
}
