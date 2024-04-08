namespace ClothesShop.ViewModels
{
    public class ReportCusVM
    {
        public string CusId { get; set; } = null!;

        public string CusName { get; set; } = null!;

        public double? CartMoney { get; set; }

        public int? CdtlQty { get; set; }
    }
}
