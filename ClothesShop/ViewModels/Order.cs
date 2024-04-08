namespace ClothesShop.ViewModels
{
    public class Order
    {
        public string CartId { get; set; } = null!;

        public string PdId { get; set; } = null!;
        public string PdName { get; set; } = null!;
        public string? CartPay { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public double? BdtlQty { get; set; }
        public double? BdtlPrice { get; set; }
        public double? BdtlMoney { get; set; }

        public string? CartSend { get; set; }

        public string? CartVoid { get; set; }
    }
}
