using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class WaitingToken
{
    public int WaitingTokenId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int? NoOfPersons { get; set; }
    public string? Phone { get; set; }


    public bool? IsActive { get; set; }

    public bool? Isassign { get; set; }
    public int? SectionId { get; set; }


    public int? UpdatedBy { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
