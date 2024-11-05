using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace momken_backend.Models
{
    [Index(nameof(RoomId))]
    public class PartnerClientRoomMessage
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();


        [Column("PartnerClientRoomId")]
        [ForeignKey(nameof(PartnerClientRoom))]
        public Guid RoomId { get; set; }  
        
        public Guid? UserId { get; set; }

        [MaxLength(100)]
        public string UserType { get; set; } 

        public string Massage { get; set; }
        public bool IsRecipientShow { get; set; } = false;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("update_at")]
        public DateTime updatAt { get; set; } = DateTime.UtcNow;

        //public PartnerClientRoom PartnerClientRoom { get; set; }
    }
}
