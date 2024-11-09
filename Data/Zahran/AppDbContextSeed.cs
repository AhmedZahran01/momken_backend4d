using momken_backend.Models; 
using System.Text.Json;

namespace momken_backend.Data.Zahran
{
    public static class AppDbContextSeed
    {
        public async static Task SeedAsync(this AppDbContext dbcontext)
        {

            if (dbcontext.Clients.Count() == 0)
            {
                var ClientsData = File.
                           ReadAllText("../momken_backend/Data/Zahran/DataSeed/ClientSeed.json");
                var client = JsonSerializer.Deserialize<List<Client>>(ClientsData);

                if (client?.Count() > 0)
                {
                    foreach (var clientfor in client)
                    {
                        dbcontext.Set<Client>().Add(clientfor);
                    }
                    await dbcontext.SaveChangesAsync();

                }
            }

            if (dbcontext.PartnerStoreTypes.Count() == 0)
            {
                var ClientsData = File.
                           ReadAllText("../momken_backend/Data/Zahran/DataSeed/PartnerStoreTypesCategories.json");
                var PartnerStoreTypes = JsonSerializer.Deserialize<List<PartnerStoreTypeCategories>>(ClientsData);

                if (PartnerStoreTypes?.Count() > 0)
                {
                    foreach (var PartnerStoreTypefor in PartnerStoreTypes)
                    {
                        dbcontext.Set<PartnerStoreTypeCategories>().Add(PartnerStoreTypefor);
                    }
                    await dbcontext.SaveChangesAsync();

                }
            }


            if (dbcontext.Partners.Count() == 0)
            {
                var CpartnersData = File.
                           ReadAllText("../momken_backend/Data/Zahran/DataSeed/partnerSeed.json");
                var PartnerStoreTypes = JsonSerializer.Deserialize<List<Partner>>(CpartnersData);

                if (PartnerStoreTypes?.Count() > 0)
                {
                    foreach (var Partnerfor in PartnerStoreTypes)
                    {
                        dbcontext.Set<Partner>().Add(Partnerfor);
                    }
                    await dbcontext.SaveChangesAsync();

                }
            }
              
            if (dbcontext.PartnerStores.Count() == 0)
            {
                var CpartnersData = File.
                           ReadAllText("../momken_backend/Data/Zahran/DataSeed/partnerStoreSeed.json");
                var PartnerStoreTypes = JsonSerializer.Deserialize<List<PartnerStore>>(CpartnersData);

                if (PartnerStoreTypes?.Count() > 0)
                {
                    foreach (var PartnerStorefor in PartnerStoreTypes)
                    {
                        dbcontext.Set<PartnerStore>().Add(PartnerStorefor);
                    }
                    await dbcontext.SaveChangesAsync();

                }
            }


            if (dbcontext.Products.Count() == 0)
            {
                var ClientsData = File.
                           ReadAllText("../momken_backend/Data/Zahran/DataSeed/PoructSeed.json");
                var PartnerStoreTypes = JsonSerializer.Deserialize<List<Product>>(ClientsData);

                if (PartnerStoreTypes?.Count() > 0)
                {
                    foreach (var PartnerStoreTypefor in PartnerStoreTypes)
                    {
                        dbcontext.Set<Product>().Add(PartnerStoreTypefor);
                    }
                    await dbcontext.SaveChangesAsync();

                }
            }


        }


    }
}
