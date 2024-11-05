using System.ComponentModel.DataAnnotations;

namespace momken_backend.Models
{
    public class PartnerStoreSubType
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        public PartnerStoreTypeCategories Type { get; set; }
         
    }
}
