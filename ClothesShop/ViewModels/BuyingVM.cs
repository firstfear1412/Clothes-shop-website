namespace ClothesShop.ViewModels
{
    public class BuyingVM
    {
        public string BuyId { get; set; } = null!;
        public string? SupId { get; set; }
        public string? SupName { get; set; }
        public DateOnly? BuyDate { get; set; }
        public string? StfId { get; set; }
        public string? BuyDocId { get; set; }
        public string? Saleman { get; set; }
        public double? BuyQty { get; set; }
        public double? BuyMoney { get; set; }
        public string? BuyRemark { get; set; }
    }
}
