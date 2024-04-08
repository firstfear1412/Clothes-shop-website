using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClothesShop.Models;

public partial class Product
{
    [Key]
    [Required(ErrorMessage = "Required Product ID")]
    [Display(Name = "Product ID")]
    public string PdId { get; set; } = null!;

    [Required(ErrorMessage = "Required Color")]
    [Display(Name = "Color")]
    public byte ColorId { get; set; }

    [Required(ErrorMessage = "Required Size")]
    [Display(Name = "Size")]
    public byte SizeId { get; set; }

    [Required(ErrorMessage = "Required Product Name")]
    [Display(Name = "Product Name")]
    public string PdName { get; set; } = null!;

    //[Required(ErrorMessage = "Required Product Detail")]
    [Display(Name = "Product Detail")]
    public string? PdDtls { get; set; }

    [Required(ErrorMessage = "Required Price")]
    [Display(Name = "Price")]
    public double? PdPrice { get; set; }

    [Required(ErrorMessage = "Required Cost")]
    [Display(Name = "Cost")]
    public double? PdCost { get; set; }

    //[Required(ErrorMessage = "Required Inventories")]
    [Display(Name = "Inventories")]
    public int? PdStk { get; set; }

    //[Required(ErrorMessage = "Required Type")]
    [Display(Name = "Type")]
    public byte? PdtId { get; set; }

    //[Required(ErrorMessage = "Required Status")]
    [Display(Name = "Status")]
    public byte? StatusId { get; set; }

    //[Required(ErrorMessage = "Required Target")]
    [Display(Name = "Target")]
    public byte? TargetId { get; set; }

    //[Required(ErrorMessage = "Required Suppliers")]
    [Display(Name = "Suppliers")]
    public string? SupId { get; set; }

    public DateOnly? PdLastBuy { get; set; }

    public DateOnly? PdLastSale { get; set; }
}
