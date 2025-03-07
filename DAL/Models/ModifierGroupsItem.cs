using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ModifierGroupsItem
{
    public int ModifierGroupItemId { get; set; }

    public int ModifierGroupId { get; set; }

    public int ItemId { get; set; }

    public int? Min { get; set; }

    public int? Max { get; set; }
}
