using momken_backend.Enums;
using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos
{
    public class UpdateStoreDto
    {
     
        public IFormFile? ImgStore { get; set; }

        
        public List<DeliveryType>? DeliveryType { get; set; }
    }
}
