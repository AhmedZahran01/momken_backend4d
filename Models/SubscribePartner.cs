using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace momken_backend.Models
{
    public class SubscribePartner
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
       public string country { get; set; }=String.Empty;
        [Required]
        public string paymentGateway {  get; set; }=String.Empty ;

        [Required]
        public string currency { get; set; }=String.Empty;

        [Required]
       public int MonthCount { get; set; }
        [Required]
        public Guid PartnerId { get; set; }
        [Required]
        public string InvoiceId {  get; set; }

        public Partner partner { get; set; }

        [Required]
        [Column("start_from")]
        public DateOnly StartFrom { get; set; } = DateOnly.FromDateTime(TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time")));

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public string amount { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("update_at")]
        public DateTime updatet { get; set; } = DateTime.UtcNow;

    }


    public class ClientDetailsOrder
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string country { get; set; } = String.Empty;
        [Required]
        public string paymentGateway { get; set; } = String.Empty;

        [Required]
        public string currency { get; set; } = String.Empty;

        [Required]
        public Guid clientId { get; set; }
        [Required]
        public string InvoiceId { get; set; }

        public Client client { get; set; } 
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public string amount { get; set; }

        [Required]
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("update_at")]
        public DateTime updatet { get; set; } = DateTime.UtcNow;

    }


}
