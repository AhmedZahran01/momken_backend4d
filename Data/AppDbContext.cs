using Microsoft.EntityFrameworkCore;
using momken_backend.Models;
using Microsoft.Extensions.Configuration; 
namespace momken_backend.Data
{
    public class AppDbContext:DbContext
    {
        private readonly IConfiguration _configuration;

        public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration configuration)
         : base(options)
        {
            _configuration = configuration;
        }



        #region  Db Sets
         
        public DbSet<Partner> Partners { get; set; }
        public DbSet<PartnerStore> PartnerStores { get; set; }
        public DbSet<PartnerStoreSubType> PartnerStoreSubTypes { get; set; }
        public DbSet<PartnerStoreTypeCategories> PartnerStoreTypes { get; set; }
        public DbSet<OTP> OTPs { get; set; }
        public DbSet<MyfatoorahTempData> MyfatoorahTempDatas { get; set; }
        public DbSet<SubscribePartner> SubscribePartner { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<PartnerClientRoom> PartnerClientRooms { get; set; }
        public DbSet<PartnerClientRoomMessage> PartnerClientRoomMessages { get; set; }

        #endregion
    
    }
    
}
