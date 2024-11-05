using momken_backend.Dtos.Myfatoorah;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace momken_backend.Models
{
    public class MyfatoorahTempData
    {
        [Key]
        public Guid Id { get; set; }=Guid.NewGuid();
        [Required]
        [Column(TypeName = "jsonb")]
        public SubscribeOrderTempData Content { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;

        [Required]
        [Column("update_at")]
        public DateTime updatet { get; set; } = DateTime.UtcNow;
    }
}
