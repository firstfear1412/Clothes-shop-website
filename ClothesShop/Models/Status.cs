using System;
using System.Collections.Generic;

namespace ClothesShop.Models;

public partial class Status
{
    public byte StatusId { get; set; }

    public string StatusName { get; set; } = null!;
}
