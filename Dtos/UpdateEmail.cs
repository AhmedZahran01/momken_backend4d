using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos
{
    public class UpdateEmail
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
