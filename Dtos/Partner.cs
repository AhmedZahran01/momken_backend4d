using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos
{
    public class Partner
    {

        [Required]
        [Phone]
        [MaxLength(16)]
        [MinLength(10)]
        public string PhoneNumper { get; set; }    
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }

        public string Password { get; set; }


    } 
    public class PartnerLogin
    {

        [Required]
        [Phone]
        [MaxLength(100)]
        public string PhoneNumper { get; set; }    

        public string Password { get; set; }


    }

    public class ClientLoginDto
    {

        [Required]
        [Phone]
        [MaxLength(100)]
        public string PhoneNumber { get; set; }

        public string Password { get; set; }


    }

    public class PartnerResetPasswoed
    {
        public string Password { get; set; }


    }
    public class PartnerSendOtp
    {

        [Required]
        [Phone]
        [MaxLength(100)]
        public string PhoneNumper { get; set; }    

    }
}
