using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos
{
    public class OTPDto
    {

        [Required]
        public string Otp { get; set; }

        [Required]
        public string MobileNumber { get; set; }
    }
}
