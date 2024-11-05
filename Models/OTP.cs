using System.ComponentModel.DataAnnotations;

namespace momken_backend.Models
{
    public class OTP
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Otp { get; set; }

        [Required]
        [MaxLength(100)]
        public string MobileNumber { get; set; }
    }
}
