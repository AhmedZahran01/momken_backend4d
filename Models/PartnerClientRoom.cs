using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace momken_backend.Models
{
    [Index(nameof(ClientId),nameof(PartnerId))]
    public class PartnerClientRoom
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClientId { get; set; }

        public Guid PartnerId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("update_at")]
        public DateTime updatAt { get; set; } = DateTime.UtcNow;

        public Client Client { get; set; }
        public Partner Partner { get; set; }

        //public List<PartnerClientRoomMessage> Messages { get; set; }





    }
}
