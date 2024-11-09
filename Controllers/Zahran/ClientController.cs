using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using momken_backend.Data;
using momken_backend.Dtos;
using momken_backend.Dtos.DataRespons;
using momken_backend.Dtos.Zahran;
using momken_backend.Models;
using momken_backend.Services;
using momken_backend.Services.Zahran;

namespace momken_backend.Controllers.Zahran
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {

        #region Constructor Region

        private readonly AppDbContext _context;
        private readonly IHashPasswordService _HashPasswordService;
        private readonly IJwtServiceclient _jwtServiceclient;

        public ClientController(AppDbContext context, IHashPasswordService HashPasswordService, IJwtServiceclient jwtServiceclient)
        {
            _context = context;
            _HashPasswordService = HashPasswordService;
            _jwtServiceclient = jwtServiceclient;
        }

        #endregion


        #region Auth Region

        #region Register Region 

        [HttpPost("singup")]
        public async Task<IActionResult> Singup([FromBody] ClientSDto client)
        {
            #region Check Phone Used Or Not Region

            var userFinded = await _context.Clients.FirstOrDefaultAsync(p => p.PhoneNumber == client.PhoneNumber);

            #region Add New Client Region

            if (userFinded == null)
            {

                var newclient = await _context.Clients.AddAsync(new Models.Client
                {
                    FirstName = client.FirstName,
                    FamilyName = client.FamilyName,
                    Email = client.Email,
                    PhoneNumber = client.PhoneNumber,
                    Password = _HashPasswordService.HashPassword(client.Password),
                });

                await _context.SaveChangesAsync();
                return Ok(new GlobalResponseDebugDto<ClientBodyResDto, OTPDto>
                {
                    success = true,
                    message = "New Client is Added Successfully",
                    data = new Dtos.DataRespons.ClientBodyResDto
                    {
                        Email = client.Email,
                        Id = newclient.Entity.Id,
                        PhoneNumber = client.PhoneNumber,

                    },
                });
            }

            #endregion

            return BadRequest(new GlobalResponseNoDataDto
            {
                success = false,
                message = "PhoneNumber  is used before "
            });


            #region Comment Full Name Not Used Region

            //var userFindedFamilyName = await _context.Clients.Where(p => p.FirstName == client.FirstName).ToListAsync();
            //if (userFindedFamilyName != null)
            //{


            //    foreach (var FamilyName in userFindedFamilyName)
            //    {
            //        if (FamilyName.FamilyName == client.FamilyName)
            //        {
            //            return BadRequest(new GlobalResponseNoDataDto
            //            {
            //                success = false,
            //                message = "Name is used before"
            //            });
            //        }
            //    }
            //}

            #endregion
          
            #region Comment Email Used Before Region

            //var userFindedEmail = await _context.Clients.Where(p => p.Email == client.Email).FirstOrDefaultAsync();
            //if (userFindedEmail is null)
            //{

            //    return BadRequest(new GlobalResponseNoDataDto
            //    {
            //        success = false,
            //        message = "PhoneNumber  is used before"
            //    });
            //}

            #endregion


            #endregion
        } 

        #endregion


        #region Login 

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Dtos.ClientLoginDto clientLogin)
        {
            var clientFound = new Client();
            if (clientLogin.PhoneNumber is not null)
            {
                clientFound = await _context.Clients.FirstOrDefaultAsync(p => p.PhoneNumber == clientLogin.PhoneNumber);

                if (clientFound != null)
                {
                    var resault = _HashPasswordService.VerifyHashedPassword(clientFound.Password, clientLogin.Password);
                    if (resault)
                    {
                        var token = _jwtServiceclient.creatJwtToken(clientFound);
                        return Ok(new GlobalResponseDebugDto<LoginClientBodyDto, string>
                        {
                            success = true,
                            message = "Login success",
                            data = new LoginClientBodyDto
                            {
                                client = new ClientBodyResDto
                                {
                                    Email = clientFound.Email,
                                    Id = clientFound.Id,
                                    PhoneNumber = clientFound.PhoneNumber,
                                },
                                Token = token
                            },
                            debug = "No debug Data"
                        });


                    }
                }
            }

            return BadRequest(new GlobalResponseNoDataDto
            {
                success = false,
                message = "Phone number or password is incorrect."
            });
        }

        #endregion

        #endregion

        
        #region كل الاقسام  اللي فيها متاجر


        [HttpGet("Get All Partner Store Types")]
        public async Task<IActionResult> GetAllPartnerStoreTypes()
        {

            var GetAllPartnerStoresTypes = await _context.PartnerStoreTypes.ToListAsync();

            var GetAllPartnerStoreTypesDtos = new List<PartnerStoreTypeCategoriesDto>();

            foreach (var type in GetAllPartnerStoresTypes)
            {
                GetAllPartnerStoreTypesDtos.Add(new PartnerStoreTypeCategoriesDto
                {
                    Id = type.Id,
                    Name = type.Name,

                });
            }

            return Ok(new GlobalResponseDebugDto<List<PartnerStoreTypeCategoriesDto>, string>
            {
                success = true,
                message = "Get All Partner Store Types Categories",
                data = GetAllPartnerStoreTypesDtos,
                debug = "No data for debug"
            });
        }

        #endregion



        #region كل المتاجر


        [HttpGet("Get All Partner Store Or Specific One By It is Id")]
        public async Task<IActionResult> GetAllPartnerStore(Guid? partnerStoreTypeById)
        {
            var GetAllPartnerStores =  _context.PartnerStores
                   .Select(store => new Dtos.Zahran.PartnerStoreDto
                   {
                        Id = store.Id,
                        City = store.City,
                        StoreName = store.StoreName,
                        ImgStore = store.ImgStore,
                        FirstName = store.FirstName,
                        FamilyName = store.FamilyName,
                        TypeId = store.TypeId,
                  
                   
                   });

            if (partnerStoreTypeById is not null)
            {
                GetAllPartnerStores = GetAllPartnerStores.Where(p => p.TypeId == partnerStoreTypeById);
            }

            return Ok(new GlobalResponseDebugDto<List<PartnerStoreDto>, string>
            {
                success = true,
                message = "Get All Partner Store",
                data = await GetAllPartnerStores.ToListAsync(),
                debug = "No data for debug"
            });
        }




        #endregion



        #region Get All Partner Store Product


        [HttpGet("All Partner Stores Products By PartnerStoreId/{PartnerStoreId}")]
        public async Task<IActionResult> GetAllPartnerStoreProduct(Guid PartnerStoreId)
        {
            var existPartnerStore = await _context.PartnerStores.FirstOrDefaultAsync(ps => ps.Id == PartnerStoreId);
            
            if (existPartnerStore is not null)
            { 
                var GetAllPartnerStoresProducts = await _context.Products.Where(c => PartnerStoreId == c.partnerStoreId)
                                                   .Include(p => p.partnerStore).ToListAsync();
 
                var ProductDtoHandle = new List<Dtos.Zahran.ProductDto>();
                if (GetAllPartnerStoresProducts.Count() == 0  )
                {
                    return Ok(new GlobalResponseDebugDto<List<Dtos.Zahran.ProductDto>, string>
                    {
                        success = true,
                        message = "There Is No Products Here", 
                        debug = "No data for debug"
                    });
                }

                foreach (var product in GetAllPartnerStoresProducts)
                {
                    ProductDtoHandle.Add(new Dtos.Zahran.ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Calories = product.Calories,
                        Allergens = product.Allergens,
                        MineImg = "public/" + product.MineImg,
                        MoreImgs = product.MoreImgs.Select(i => "public/" + i).ToArray(),
                        deletedtAt = product.deletedtAt,
                        partnerStoreId = product.partnerStoreId,

                    });
                }
                
                return Ok(new GlobalResponseDebugDto<List<Dtos.Zahran.ProductDto>, string>
                {
                    success = true,
                    message = " All Partner Store Products ",
                    data = ProductDtoHandle,
                    debug = "No data for debug"
                });
            }
            return BadRequest(new GlobalResponseNoDataDto
            {
                success = false,
                message = "A Bad Request you have Made , There is No Partner Store Like this "
            });
 
        }


        #endregion


        #region Get Product Details By Id


        [HttpGet("GetProductById/{ProductId}")]
        public async Task<IActionResult> GetProductById(Guid ProductId)
        {

            var GetAllPartnerStores = await _context.Products.Where(p => ProductId == p.Id).ToListAsync();
            if (GetAllPartnerStores.Count() != 0)
            {
                var ProductDtoHandle = new List<Dtos.Zahran.ProductDto>();
                foreach (var product in GetAllPartnerStores)
                {
                    ProductDtoHandle.Add(new Dtos.Zahran.ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        Calories = product.Calories,
                        Allergens = product.Allergens,
                        MineImg = product.MineImg,
                        MoreImgs = product.MoreImgs,
                        deletedtAt = product.deletedtAt,
                        partnerStoreId = product.partnerStoreId,

                    });
                }
                return Ok(new GlobalResponseDebugDto<List<Dtos.Zahran.ProductDto>, string>
                {
                    success = true,
                    message = "Get All Partner Store Types Category",
                    data = ProductDtoHandle,
                    debug = "No data for debug"
                });
            }

            return BadRequest(new GlobalResponseNoDataDto
            {
                success = false,
                message = "A Bad Request you have Made , There is No Product Like this "
            });


        }


        #endregion


    }
}
