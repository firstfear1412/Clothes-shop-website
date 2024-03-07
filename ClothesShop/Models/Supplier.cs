using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClothesShop.Models;

public partial class Supplier
{
    [Key]
    [Required(ErrorMessage = "ต้องระบุรหัส")]
    [Display(Name = "SupId")]
    public string SupId { get; set; } = null!;

    public string? SupName { get; set; }

    public string? SupContact { get; set; }

    public string? SupTel { get; set; }

    public string? SupEmail { get; set; }

    public string? SupAdd { get; set; }

    public string? SupRemark { get; set; }
}
