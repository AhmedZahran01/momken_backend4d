using momken_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos.DataRespons
{
    public class ProfileDto
    {
        public string PhoneNumper { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string FamilyName { get; set; }


        public string IDNumber { get; set; }

        public string NumperComOrFreeRegister { get; set; }


        public string NameComOrFreeRegister { get; set; }

        public string DateStartComOrFreeRegister { get; set; }

        public string DateEndComOrFreeRegister { get; set; }


        public string StoreName { get; set; }


        public string Type { get; set; }


        public string ImgStore { get; set; }

        public int[] DeliveryType { get; set; }

        public string City { get; set; }
        public string ImgNationalID { get; set; }
        public string EmgComOrFreeRegister {get;set;}

    }
}
