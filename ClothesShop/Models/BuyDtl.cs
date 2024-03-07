using System;
using System.Collections.Generic;

namespace ClothesShop.Models;

public partial class BuyDtl
{
    public string BuyId { get; set; } = null!;

    public string PdId { get; set; } = null!;

    public int? BdtlQty { get; set; }

    public double? BdtlPrice { get; set; }

    public double? BdtlMoney { get; set; }
}
