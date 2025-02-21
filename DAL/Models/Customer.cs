using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? NoOfPersons { get; set; }

    public bool? IsActive { get; set; }

    public int? UpdatedBy { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Phone { get; set; }

    public virtual ICollection<AllocatedTable> AllocatedTables { get; } = new List<AllocatedTable>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual ICollection<Payment> Payments { get; } = new List<Payment>();

    public virtual User? UpdatedByNavigation { get; set; }
}
