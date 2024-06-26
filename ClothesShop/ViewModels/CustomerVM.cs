﻿namespace ClothesShop.ViewModels
{
    public class CustomerVM
    {
        public string CusId { get; set; } = null!;

        public string CusName { get; set; } = null!;

        public string CusLogin { get; set; } = null!;

        public string CusPass { get; set; } = null!;

        public string CusEmail { get; set; } = null!;

        public string? CusAdd { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? LastLogin { get; set; }
    }
}
