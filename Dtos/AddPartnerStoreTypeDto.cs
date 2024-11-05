using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos
{
    public class AddPartnerStoreTypeDto
    {
        [Required]
        public List<string> types { get; set; }
    }
}
