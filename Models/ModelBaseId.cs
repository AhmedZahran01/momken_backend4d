using System.ComponentModel.DataAnnotations;

namespace momken_backend.Models
{
    public class ModelBaseId
    {

        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
