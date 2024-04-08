using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClothesShop.Models;

public partial class Customer
{
    public string CusId { get; set; } = null!;

    //[Required(ErrorMessage = "ชื่อ")]
    [Display(Name = "Customer Name")]
    public string CusName { get; set; } = null!;

    [Display(Name = "Username ID")]
    public string CusLogin { get; set; } = null!;

    [Display(Name = "Passwoed")]
    public string CusPass { get; set; } = null!;

    [Display(Name = "E-mail")]
    public string CusEmail { get; set; } = null!;

    [Display(Name = "Address ")]
    public string? CusAdd { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? LastLogin { get; set; }
}
