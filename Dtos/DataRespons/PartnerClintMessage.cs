using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos.DataRespons
{
    public class PartnerClintMessage
    {
 
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }
        public bool IsYour { get; set; }
        public string Massage { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime updatAt { get; set; } 

    }
}
