using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace momken_backend.Models
{
    public class Partner : ModelBaseId
    {

        #region Properties Region

        [Required]    [Phone]            [MaxLength(100)]
        public string PhoneNumber { get; set; }
        [Required]    [EmailAddress]     [MaxLength(255)]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool PhoneNumberVerifed { get; set; } = false;
         
        #endregion

        public List<PartnerStore>? PartnerStore { get; set; }
         
    }
}
