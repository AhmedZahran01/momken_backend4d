using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos
{
    public class ProductUpdateDto
    {
     
        public string? Name { get; set; }

       
        public string? Description { get; set; }

      
        public int? Price { get; set; }
      
        public IFormFile? MineImg { get; set; }
        public Guid? Type { get; set; }
        public Guid? SubType { get; set; }

        public IFormFile[]? MoreImgs { get; set; }
        public string? Calories { get; set; } = String.Empty;
        public string? Allergens { get; set; } = String.Empty;
    }
}
