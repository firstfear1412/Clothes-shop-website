namespace ClothesShop.ViewModels
{
    public class CtdVM
    {
        public string CartId { get; set; } = null!;

        public string PdId { get; set; } = null!;
        
        public string PdName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }

        public double? CdtlQty { get; set; }

        public double? CdtlPrice { get; set; }

        public double? CdtlMoney { get; set; }
    }
}
