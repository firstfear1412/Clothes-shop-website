using System;
using System.Collections.Generic;

namespace ClothesShop.Models;

public partial class CartDtl
{
    public string CartId { get; set; } = null!;

    public string PdId { get; set; } = null!;

    public int? CdtlQty { get; set; }

    public double? CdtlPrice { get; set; }

    public double? CdtlMoney { get; set; }
}
