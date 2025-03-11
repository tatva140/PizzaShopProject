using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ModifierModifierGroup
{
    public int ModifierModifierGroupsId { get; set; }

    public int ModifierGroupId { get; set; }

    public int ModifierId { get; set; }
}
