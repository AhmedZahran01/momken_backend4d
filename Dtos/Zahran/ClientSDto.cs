using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos.Zahran
{
    public class ClientSDto
    {

        
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        [Required]
        [Phone]
        [MaxLength(16)]
        [MinLength(9)]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }
        public string Password { get; set; } 

         
    }
}
