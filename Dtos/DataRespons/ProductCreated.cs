using System.ComponentModel.DataAnnotations;

namespace momken_backend.Dtos.DataRespons
{
    public class ProductCreated
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        
        public string Description { get; set; }

       
        public int Price { get; set; }
     
        public string MineImg { get; set; }

        public string[] MoreImgs { get; set; }

        public string Type { get; set; }
        public Guid? TypeId { get; set; }
        public string Calories { get; set; } = String.Empty;
        public string Allergens { get; set; } = String.Empty;
    }
}
