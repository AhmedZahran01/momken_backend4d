using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos.Zahran
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public string Calories { get; set; } = String.Empty;
        public string Allergens { get; set; } = String.Empty;

        public string MineImg { get; set; }

        public string[] MoreImgs { get; set; }
        [Column("deleted_at")]
        public DateTime? deletedtAt { get; set; }



        public Guid? partnerStoreId { get; set; }
 
    }
}
