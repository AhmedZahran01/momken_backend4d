using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos.Zahran
{
    public class PartnerStoreDto
    {
        public Guid Id { get; set; }
 
        [Required]
        [MaxLength(255)]
        public string City { get; set; }
        [MaxLength(255)]
        public string FirstName { get; set; }
        [MaxLength(255)]
        public string FamilyName { get; set; }
        [Required]
        [MaxLength(100)]
        public string IDNumber { get; set; }
        public string ImgNationalID { get; set; }
        [Required]
        [MaxLength(255)]
        public DateOnly DateStartComOrFreeRegister { get; set; }
        public DateOnly DateEndComOrFreeRegister { get; set; }
        [Required]
        public string ImgStore { get; set; }
        public string NumberComOrFreeRegister { get; set; }
        [Required]
        [MaxLength(255)]
        public string NameComOrFreeRegister { get; set; }
        public string EmgComOrFreeRegister { get; set; }
        [Required]
        [MaxLength(255)]
        public string StoreName { get; set; }

        [Required]
        public int[] DeliveryType { get; set; }




        public Guid? TypeId { get; set; }


    }
}
