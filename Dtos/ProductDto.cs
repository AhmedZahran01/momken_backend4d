using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos
{
    public class ProductDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }
        [Required]
        public IFormFile MineImg { get; set; }
        public Guid Type {  get; set; } 
        public Guid? SubType {  get; set; } 

        public IFormFile[]? MoreImgs { get; set; }
        public string Calories { get; set; } = String.Empty;
        public string Allergens { get; set; } = String.Empty;
    }
}
