namespace ClothesShop.ViewModels
{
    public class BdVM
    {
        public string BuyId { get; set; } = null!;
        public string PdId { get; set; } = null!;
        public string PdName { get; set; } = null!;
        public double? BdtlQty { get; set; }
        public double? BdtlPrice { get; set; }
        public double? BdtlMoney { get; set; }
    }
}
