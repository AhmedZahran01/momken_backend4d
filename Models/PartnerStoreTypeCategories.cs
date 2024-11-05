using System.ComponentModel.DataAnnotations;

namespace momken_backend.Models
{
    public class PartnerStoreTypeCategories
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        public List<PartnerStore>?  Products { get; set; }
        
      
        //public List<PartnerStoreSubType>?  SubTypes { get; set; }


    }
}
