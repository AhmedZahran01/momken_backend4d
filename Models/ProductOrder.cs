using System.ComponentModel.DataAnnotations;

namespace momken_backend.Models
{
    public class ProductOrder:ModelBaseId
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; } 
        [Required]
        public string Description { get; set; }
        public int? Quanttity { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public string Calories { get; set; } = String.Empty;
        public string Allergens { get; set; } = String.Empty;

        public string MineImg { get; set; }

        public string[] MoreImgs { get; set; }

    }
}
