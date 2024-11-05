using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos.Zahran
{
    public class PartnerStoreTypeCategoriesDto
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
