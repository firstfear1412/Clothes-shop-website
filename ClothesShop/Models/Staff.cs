using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClothesShop.Models;

public partial class Staff
{
    [Display(Name = "Employee ID")]
    public string StfId { get; set; } = null!;


    [Display(Name = "Employee Name")]
    public string StfName { get; set; } = null!;

    [Display(Name = "Passwoed")]
    public string StfPass { get; set; } = null!;

    [Display(Name = "Duty")]
    public string? DutyId { get; set; } = null!;

    public DateOnly? StartDate { get; set; }

    public DateOnly? QuitDate { get; set; }
}
