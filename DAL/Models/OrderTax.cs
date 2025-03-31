using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class OrderTax
{
    public int OrderTaxId { get; set; }

    public int? OrderId { get; set; }

    public int? TaxId { get; set; }
    public decimal TaxAmount { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Tax? Tax { get; set; }
}
