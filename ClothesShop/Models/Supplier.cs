using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClothesShop.Models;

public partial class Supplier
{
    [Key]
    [Required(ErrorMessage = "ต้องระบุรหัส")]
    [Display(Name = "Suppliers ID")]
    public string SupId { get; set; } = null!;

    [Display(Name = "Suppliers Name")]
    public string? SupName { get; set; }

    [Display(Name = "Contact")]
    public string? SupContact { get; set; }

    [Display(Name = "Tel")]
    public string? SupTel { get; set; }
    [Display(Name = "E-mail")]
    public string? SupEmail { get; set; }
    [Display(Name = "Suppliers Address ")]
    public string? SupAdd { get; set; }

    public string? SupRemark { get; set; }
}
