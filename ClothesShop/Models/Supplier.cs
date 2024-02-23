using System;
using System.Collections.Generic;

namespace ClothesShop.Models;

public partial class Supplier
{
    public string SupId { get; set; } = null!;

    public string? SupName { get; set; }

    public string? SupContact { get; set; }

    public string? SupTel { get; set; }

    public string? SupEmail { get; set; }

    public string? SupAdd { get; set; }

    public string? SupRemark { get; set; }
}
