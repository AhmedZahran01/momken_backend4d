using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos.Zahran
{
    public class ClientSDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string FamilyName { get; set; }
        [Required]
        [Phone]
        [MaxLength(16)]
        [MinLength(8)]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; } 

         
    }
}
