using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos.DataRespons
{
    public class PartnerBodyResDto
    {
        public Guid Id { get; set; }

        public string PhoneNumper { get; set; }
        public string Email { get; set; }

    }
}
