using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClothesShop.Models;

public partial class Product
{
    [Key]
    [Required(ErrorMessage = "ต้องระบุรหัสสินค้า")]
    [Display(Name = "รหัสสินค้า")]
    public string PdId { get; set; } = null!;

    [Required(ErrorMessage = "ต้องระบุสีสินค้า")]
    [Display(Name = "สีสินค้า")]
    public byte ColorId { get; set; }

    [Required(ErrorMessage = "ต้องระบุขนาดสินค้า")]
    [Display(Name = "ขนาดสินค้า")]
    public byte SizeId { get; set; }

    [Required(ErrorMessage = "ต้องระบุชื่อสินค้า")]
    [Display(Name = "ชื่อสินค้า")]
    public string PdName { get; set; } = null!;

    [Display(Name = "รายละเอียดสินค้า")]
    public string? PdDtls { get; set; }

    [Display(Name = "ราคาสินค้า")]
    public double? PdPrice { get; set; }

    [Display(Name = "ต้นทุน")]
    public double? PdCost { get; set; }

    [Display(Name = "คงเหลือ")]
    public byte? PdStk { get; set; }

    [Display(Name = "ประเภทสินค้า")]
    public byte? PdtId { get; set; }

    [Display(Name = "สถานะ")]
    public byte? StatusId { get; set; }

    [Display(Name = "กลุ่มเป้าหมาย")]
    public byte? TargetId { get; set; }

    [Display(Name = "ผู้ผลิต")]
    public byte? SupId { get; set; }

    public DateOnly? PdLastBuy { get; set; }

    public DateOnly? PdLastSale { get; set; }
}
