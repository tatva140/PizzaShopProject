using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Review
{
    public int ReviewId { get; set; }

    public string? Comments { get; set; }

    public int? Food { get; set; }

    public int? Service { get; set; }

    public int? Ambience { get; set; }
}
