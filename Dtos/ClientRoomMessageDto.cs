using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos
{
    public class ClientRoomMessageDto
    {
        [Required]
       public Guid ClientId { get; set; }
        [Required]
        public Guid  PartnerId { get; set; }
        [Required]
        public string  Massage { get; set; }
    }
}
