namespace ClothesShop.ViewModels
{
    public class OrderDtl
    {
        public string CartId { get; set; } = null!;

        public string? CusName { get; set; }

        public DateOnly? CartDate { get; set; }

        public double? CartMoney { get; set; }

        public int? CartQty { get; set; }

        public string? CartPay { get; set; }

        public string? CartSend { get; set; }

        public string? CartVoid { get; set; }


    }
}
