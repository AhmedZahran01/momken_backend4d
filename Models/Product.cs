using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace momken_backend.Models
{

    public class Product : ModelBase
    {
        #region Properties Region
  
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public string Calories { get; set; } = String.Empty;
        public string Allergens { get; set; } = String.Empty;

        public string MineImg { get; set; }

        public string[] MoreImgs { get; set; }
        [Column("deleted_at")]
        public DateTime? deletedtAt { get; set; }

        #endregion


        #region Navigational Properties Region


        [ForeignKey(nameof(Models.PartnerStoreTypeCategories))]
        public Guid? TypeId { get; set; }

        public Guid? partnerStoreId { get; set; }
        public PartnerStore? partnerStore { get; set; }

        public PartnerStoreTypeCategories? Type { get; set; }


        //[ForeignKey(nameof(Models.PartnerStoreSubType))]
        //public Guid? SubTypeId { get; set; }
        //public PartnerStoreSubType? SubType { get; set; }




        public Guid partnerId { get; set; }  
        public Partner partner { get; set; } 
        
       

      
        #endregion 

    }
}
