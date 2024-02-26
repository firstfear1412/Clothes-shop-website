namespace ClothesShop.ViewModels
{
    public class DtlsVM
    {
        public string PdId { get; set; } = null!;
        public byte ColorId { get; set; }
        public string ColorName { get; set; }
        public string PdName { get; set; } = null!;
        public byte SizeId { get; set; }
        public string SizeName { get; set; }
        public double? PdStk { get; set; }
        public double? PdPrice { get; set; }
    }
}
