using System;
using System.Collections.Generic;

namespace ClothesShop.Models;

public partial class Cart
{
    public string CartId { get; set; } = null!;

    public string? CusId { get; set; }

    public DateOnly? CartDate { get; set; }

    public double? CartMoney { get; set; }

    public int? CartQty { get; set; }

    public string? CartCf { get; set; }

    public string? CartPay { get; set; }

    public string? CartSend { get; set; }

    public string? CartVoid { get; set; }
}
