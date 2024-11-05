using momken_backend.Enums;
using System.ComponentModel.DataAnnotations;
using momken_backend.Models;
using Microsoft.AspNetCore.Http;
namespace momken_backend.Dtos
{
    public class PartnerStore
    {

        [MaxLength(255)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        public string FamilyName { get; set; }

        [Required]
        [MaxLength(100)]
        public string IDNumber { get; set; }

        [Required(ErrorMessage = "Please select a file.")]
        [DataType(DataType.Upload)]
        public IFormFile ImgNationalID { get; set; }

        [Required]
        [MaxLength(255)]
        public string NumperComOrFreeRegister { get; set; }

        [Required]
        [MaxLength(255)]
        public string NameComOrFreeRegister { get; set; }

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)] // Specifies that this is a date field
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string DateStartComOrFreeRegister { get; set; }
        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)] // Specifies that this is a date field
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string DateEndComOrFreeRegister { get; set; }
        [Required(ErrorMessage = "Date is required")]
        public IFormFile EmgComOrFreeRegister { get; set; }

        [Required]
        [MaxLength(255)]
        public string StoreName { get; set; }

        [Required]
        public Guid Type { get; set; }


        public Guid? SubType { get; set; }

        [Required]
        public IFormFile ImgStore { get; set; }

        [Required]
        public List<DeliveryType> DeliveryType { get; set; }
        [Required]
        public string City { get; set;}

    }
}
