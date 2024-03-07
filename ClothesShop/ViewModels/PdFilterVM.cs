using System.ComponentModel.DataAnnotations;

namespace ClothesShop.ViewModels
{
    public class PdFilterVM
    {
        public string PdId { get; set; } = null!;

        public byte? ColorId { get; set; }
        public string? ColorName { get; set; }

        public byte? SizeId { get; set; }
        public string? SizeName { get; set; }

        public string PdName { get; set; } = null!;
        public string? PdDtls { get; set; }

        public double? PdPrice { get; set; }

        public double? PdCost { get; set; }

        public double? PdStk { get; set; }

        public byte? PdtId { get; set; }
        public string? PdtName { get; set; }

        public byte? StatusId { get; set; }

        public string? StatusName { get; set; }



        public byte? TargetId { get; set; }

        public string? TargetName { get; set; }


        public byte? SupId { get; set; }

    }
}
