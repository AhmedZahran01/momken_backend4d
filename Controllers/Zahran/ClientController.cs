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

        public ClientController(AppDbContext context, IHashPasswordService HashPasswordService , IJwtServiceclient jwtServiceclient)
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

            var userFinded = await _context.Clients.Where(p => p.PhoneNumber == client.PhoneNumber).FirstOrDefaultAsync();

            if (userFinded != null)
            {
                return BadRequest(new GlobalResponseNoDataDto
                {
                    success = false,
                    message = "PhoneNumber  is used before"
                });
            } 

            #endregion

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

            #region Add New Client Region
           
            var newclient = await _context.Clients.AddAsync(new Models.Client
            {
                FirstName = client.FirstName,
                FamilyName = client.FamilyName,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Password = _HashPasswordService.HashPassword(client.Password),
                Id = Guid.NewGuid()
            });

            await _context.SaveChangesAsync();
            return Ok(new GlobalResponseDebugDto<PartnerBodyResDto, OTPDto>
            {
                success = true,
                message = "Added new client",
                data = new Dtos.DataRespons.PartnerBodyResDto
                {
                    Email = client.Email,
                    Id = newclient.Entity.Id,
                    PhoneNumper = client.PhoneNumber,

                },
            });

            #endregion
       

        }

        #endregion


        #region Login 

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Dtos.PartnerLogin clientLogin)
        {
            var clientFound = await _context.Clients.FirstOrDefaultAsync(p => p.PhoneNumber == clientLogin.PhoneNumper);
            
            if (clientFound == null)
            {
                return BadRequest(new GlobalResponseNoDataDto
                {
                    success = false,
                    message = "PhoneNumper or passwors wroung"
                });
            } 

            var resault = _HashPasswordService.VerifyHashedPassword(clientFound.Password, clientLogin.Password);
            if (!resault)
            {
                return BadRequest(new GlobalResponseNoDataDto
                {
                    success = false,
                    message = "PhoneNumper or passwors wroung"
                });
            }
            var token = _jwtServiceclient.creatJwtToken(clientFound);
            return Ok(new GlobalResponseDebugDto<LoginBodyDto, string>
            {
                success = true,
                message = "Login success",
                data = new LoginBodyDto
                {
                    Partner = new PartnerBodyResDto
                    {
                        Email = clientFound.Email,
                        Id = clientFound.Id,
                        PhoneNumper = clientFound.PhoneNumber,
                    },
                    Token = token
                },
                debug = "No debug Data"
            });

        }

        #endregion

        #endregion

      
        #region كل الاقسام


        [HttpGet("AllStoreTyoes")]
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
                message = "Get All Partner Store Types Category",
                data = GetAllPartnerStoreTypesDtos,
                debug = "No data for debug"
            });
        }

        #endregion


        #region كل المتاجر


        [HttpGet("GetAllPartnerStore")]
        public async Task<IActionResult> GetAllPartnerStore(Guid? partnerStoreTypeById)
        {
                var GetAllPartnerStoreDto = new List<PartnerStoreDto>();
            

                var GetAllPartnerStores =   _context.PartnerStores.Select(store => new Dtos.Zahran.PartnerStoreDto
                {
                    Id = store.Id,
                    City = store.City,
                    DateEndComOrFreeRegister = store.DateEndComOrFreeRegister,
                    DateStartComOrFreeRegister = store.DateStartComOrFreeRegister,
                    StoreName = store.StoreName,
                    ImgStore = store.ImgStore,
                    DeliveryType = store.DeliveryType,
                    EmgComOrFreeRegister = store.EmgComOrFreeRegister,
                    FamilyName = store.FamilyName,
                    FirstName = store.FirstName,
                    IDNumber = store.IDNumber,
                    ImgNationalID = store.ImgNationalID,
                    NameComOrFreeRegister = store.NameComOrFreeRegister,
                    TypeId = store.TypeId,
                    NumberComOrFreeRegister = store.NumberComOrFreeRegister

                });
        
             if(partnerStoreTypeById is not null)
             {
                GetAllPartnerStores.Where( p => p.TypeId == partnerStoreTypeById);
             }


            return Ok(new GlobalResponseDebugDto<List<PartnerStoreDto>, string>
            {
                success = true,
                message = "Get All Partner Store ",
                data = await GetAllPartnerStores.ToListAsync(),
                debug = "No data for debug"
            });
        }




        #endregion



        #region Get All Partner Store Product


        [HttpGet("AllPartnerStoresBasedOnPartnerStoreId/{PartnerStoreId}")]
        public async Task<IActionResult> GetAllPartnerStoreProduct(Guid PartnerStoreId)
        {

            var GetAllPartnerStores = await _context.Products.Where(c => PartnerStoreId == c.partnerStoreId).Include(p => p.partnerStore)
                                                  .ToListAsync();

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
                    MineImg = "public/"+product.MineImg,
                    MoreImgs =  product.MoreImgs.Select(i => "public/" + i).ToArray(),
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


        #endregion


        #region Get Product Details By Id


        [HttpGet("GetProductById/{ProductId}")]
        public async Task<IActionResult> GetProductById(Guid ProductId)
        {

            var GetAllPartnerStores = await _context.Products.Where(p => ProductId == p.Id).ToListAsync();

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


        #endregion


    }
}
