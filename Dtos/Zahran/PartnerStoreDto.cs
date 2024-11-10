using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos.Zahran
{
    public class PartnerStoreDto
    {
        public Guid Id { get; set; }
        [Required]       [MaxLength(255)]
        public string StoreName { get; set; }

        [Required]    [MaxLength(255)]
        public string City { get; set; }
        [MaxLength(255)]   [Required]
        public string FirstName { get; set; }
        [MaxLength(255)]  [Required]
        public string FamilyName { get; set; }
        [Required]
        public int[] DeliveryType { get; set; }
        public Guid? TypeId { get; set; }
        public string? TypeName { get; set; }
        public string ImgStore { get; set; }

        #region commented Properties
        //[Required]
        //[MaxLength(100)]
        //public string IDNumber { get; set; }
        //public string ImgNationalID { get; set; }
        //[Required]
        //[MaxLength(255)]
        //public DateOnly DateStartComOrFreeRegister { get; set; }
        //public DateOnly DateEndComOrFreeRegister { get; set; }

        //public string NumberComOrFreeRegister { get; set; }
        //[Required]
        //[MaxLength(255)]
        //public string NameComOrFreeRegister { get; set; }
        //public string EmgComOrFreeRegister { get; set; }

        #endregion
    }
}
