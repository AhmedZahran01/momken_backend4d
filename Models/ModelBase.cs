using System.ComponentModel.DataAnnotations.Schema;

namespace momken_backend.Models
{
    public class ModelBase : ModelBaseId
    {
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("update_at")]
        public DateTime updatAt { get; set; } = DateTime.UtcNow;
    }
}
