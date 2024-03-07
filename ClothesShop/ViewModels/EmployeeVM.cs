namespace ClothesShop.ViewModels
{
    public class EmployeeVM
    {
        public string StfId { get; set; } = null!;

        public string StfName { get; set; } = null!;

        public string StfPass { get; set; } = null!;

        public string? DutyId { get; set; }

        public DateOnly? StartDate { get; set; }

        public DateOnly? QuitDate { get; set; }
    }
}
