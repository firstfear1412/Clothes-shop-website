//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

//namespace ClothesShop.Models;

//public partial class BuyDtl
//{
//    public string BuyId { get; set; } = null!;

//    public string PdId { get; set; } = null!;

//    public int? BdtlQty { get; set; }

//    public double? BdtlPrice { get; set; }

//    public double? BdtlMoney { get; set; }
//}


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClothesShop.Models;

public partial class BuyDtl
{
    [Key]
    [Display(Name = "Buying ID")]
    public string BuyId { get; set; } = null!;

    [Display(Name = "Product")]
    public string PdId { get; set; } = null!;
    [Display(Name = "Quantity")]
    public int? BdtlQty { get; set; }

    [Display(Name = "Price / piece")]
    public double? BdtlPrice { get; set; }

    [Display(Name = "Total Price")]
    public double? BdtlMoney { get; set; }
}