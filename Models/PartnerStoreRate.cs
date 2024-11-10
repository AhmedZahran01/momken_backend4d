
using momken_backend.Models;

namespace momken_backend4d.Models
{
    public class PartnerStoreRate
    {
        public Guid partnerStoreId { get; set; }
        public Guid clientId { get; set; }

        public decimal partnerStoreRate { get; set; }

        public PartnerStore partnerStore { get; set; }
        public Client client { get; set; }
    }
}
