using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos
{
    public class SubscribeOrderDto
    {
        [Required]
      
        public int MonthCount { get; set; } 
        
        [Required]
        public int paymentMethodId { get; set; }
    }
}
