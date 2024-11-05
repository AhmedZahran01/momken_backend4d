using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace momken_backend.Models
{
    public class Client : ModelBase
    {
      
        #region Properties Region 
        public string FirstName { get; set; }
        public string FamilyName { get; set; }
        [Required]  [Phone] [MaxLength(100)]
        public string PhoneNumber { get; set; }
          [Required]      [EmailAddress]     [MaxLength(255)]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool PhoneNumberVerifed { get; set; } = false;


        #endregion

      
        #region Navigational Properties Region




        #endregion

    }
}
