using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using momken_backend.Data;
using momken_backend.Dtos;
using System.Text;
using Microsoft.AspNetCore.Identity;
using momken_backend.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using momken_backend.Models;
using momken_backend.Dtos.DataRespons;
using Microsoft.AspNetCore.Http.HttpResults;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using momken_backend.Hubs;
using System.Net.NetworkInformation;
using momken_backend.Enums;

namespace momken_backend.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IHashPasswordService _HashPasswordService;
        private readonly IJwtServicePartner _jwtServicePartner;
        private readonly IUploadFileService _uploadFileService;
        private readonly ILogger<AuthController> _logger;
        private readonly ITempImgService _tempImgService;
        public AuthController(AppDbContext context,IHashPasswordService HashPasswordService, IJwtServicePartner jwtServicePartner, IUploadFileService uploadFileService, ILogger<AuthController>  logger, ITempImgService tempImgService)
        {
            _context = context;
            _HashPasswordService = HashPasswordService;
            _jwtServicePartner = jwtServicePartner;
            _uploadFileService = uploadFileService;
            _logger = logger;
            _tempImgService = tempImgService;
        }

        [HttpPost("singup")]
        public async Task<IActionResult> Singup([FromBody] Dtos.Partner partner)
        {
            var userFinded = await _context.Partners.Where(p => p.PhoneNumber == partner.PhoneNumper || p.Email == partner.Email).
                FirstOrDefaultAsync();
            if (userFinded != null)
            {
                return BadRequest(new GlobalResponseNoDataDto
                {
                    success=false,
                    message= "PhoneNumper or Email is used"
                });
            }
            var newPartner = await _context.Partners.AddAsync(new Models.Partner
            {
                Email = partner.Email,
                Password = _HashPasswordService.HashPassword(partner.Password),
                PhoneNumber = partner.PhoneNumper,
                Id = Guid.NewGuid()
            });

            Random rnd = new Random();
            int otp = rnd.Next(100, 999);
           var newOtp=  await _context.OTPs.AddAsync(new Models.OTP
            {
                Id=Guid.NewGuid(),
                MobileNumber=partner.PhoneNumper,
                Otp=otp.ToString()
            });
            await _context.SaveChangesAsync();
            return Ok(new GlobalResponseDebugDto<PartnerBodyResDto, OTPDto>
            {
                success= true,
                message= "Added new Partner",
                data=new Dtos.DataRespons.PartnerBodyResDto
                {
                    Email=partner.Email,
                    Id= newPartner.Entity.Id,
                    PhoneNumper=partner.PhoneNumper,

                },
                debug= new OTPDto
                {
                    MobileNumber=partner.PhoneNumper,
                    Otp= otp.ToString()
                }
            });

        }

        [HttpPost("login")]
        public  async Task<IActionResult> Login([FromBody] Dtos.PartnerLogin partner)
        {
          var partnerFound = await  _context.Partners.FirstOrDefaultAsync(p=>p.PhoneNumber==partner.PhoneNumper);
            if (partnerFound == null)
            {
                return BadRequest(new GlobalResponseNoDataDto
                {
                    success=false,
                    message= "PhoneNumper or passwors wroung"
                });
            }
         var resalt = _HashPasswordService.VerifyHashedPassword(partnerFound.Password, partner.Password);
            if (!resalt) {
                return BadRequest(new GlobalResponseNoDataDto
                {
                    success = false,
                    message = "PhoneNumper or passwors wroung"
                });
            }
          var token =  _jwtServicePartner.creatJwtToken(partnerFound);
            return Ok(new GlobalResponseDebugDto<LoginBodyDto, string>
            {
                success=true,
                message="Login success",
                data= new LoginBodyDto { Partner= new PartnerBodyResDto
                {
                    Email= partnerFound.Email,
                    Id=partnerFound.Id,
                    PhoneNumper=partnerFound.PhoneNumber,
                } ,
                Token=token},
                debug="No debug Data"
            });

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var nameIdentifierClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var userId = Guid.Parse(nameIdentifierClaim.Value);
           var partner = await _context.Partners.Where(p => p.Id == userId)
                   .Select(p => new ProfileDto
                   {
                       Email = p.Email,
                       PhoneNumper = p.PhoneNumber,
                       FirstName = p.PartnerStore.FirstOrDefault().FirstName,
                       FamilyName = p.PartnerStore.FirstOrDefault().FamilyName,
                       City = p.PartnerStore.FirstOrDefault().City,
                       IDNumber = p.PartnerStore.FirstOrDefault().IDNumber,
                       DateEndComOrFreeRegister = p.PartnerStore.FirstOrDefault().DateEndComOrFreeRegister.ToString(),
                       DateStartComOrFreeRegister = p.PartnerStore.FirstOrDefault().DateStartComOrFreeRegister.ToString(),
                       DeliveryType = p.PartnerStore.FirstOrDefault().DeliveryType,
                       ImgStore = p.PartnerStore.FirstOrDefault().ImgStore,
                       NameComOrFreeRegister = p.PartnerStore.FirstOrDefault().NameComOrFreeRegister,
                       NumperComOrFreeRegister = p.PartnerStore.FirstOrDefault().NumberComOrFreeRegister,
                       StoreName = p.PartnerStore.FirstOrDefault().StoreName,
                       Type = p.PartnerStore.FirstOrDefault().Type.Name,
                       EmgComOrFreeRegister = p.PartnerStore.FirstOrDefault().EmgComOrFreeRegister,
                       ImgNationalID = p.PartnerStore.FirstOrDefault().ImgNationalID,
                   })
                .FirstOrDefaultAsync();
            if (partner == null)
            {
                return BadRequest(new GlobalResponseNoDataDto
                {
                    success = true,
                    message= "partner Not Found"
                });
            }
           //var partnerStore = partner.PartnerStore.FirstOrDefault();
            //if (partnerStore == null) {
            //    return BadRequest(new GlobalResponseNoDataDto
            //    {
            //        success = true,
            //        message = "partnerStore Not Found"
            //    });
            //}
            string ImgStore; 
            if (partner.ImgStore == null) {
                ImgStore = "";
            }
            else
            {
                ImgStore= partner.ImgStore;
            } string type; 
            if (partner.Type == null) {
                type = "";
            }
            else
            {
                type = partner.Type;
            }

            return Ok(new ProfileDto
            {
                Email = partner.Email,
                PhoneNumper = partner.PhoneNumper,
                FirstName = partner.FirstName,
                FamilyName = partner.FamilyName,
                City = partner.City,
                IDNumber = partner.IDNumber,
                DateEndComOrFreeRegister = partner.DateEndComOrFreeRegister.ToString(),
                DateStartComOrFreeRegister = partner.DateStartComOrFreeRegister.ToString(),
                DeliveryType = partner.DeliveryType,
                ImgStore = "public/" + ImgStore,
                NameComOrFreeRegister = partner.NameComOrFreeRegister,
                NumperComOrFreeRegister = partner.NumperComOrFreeRegister,
                StoreName = partner.StoreName,
                Type = type,
                EmgComOrFreeRegister =  _tempImgService.Upload(partner.EmgComOrFreeRegister),
                ImgNationalID= _tempImgService.Upload(partner.ImgNationalID),


            });
        } 
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("create_store")]
        [RequestSizeLimit(100*1024*1024)]
        public async Task<IActionResult> CreateStore([FromForm] Dtos.PartnerStore partnerStore)
        {
           var partnerStoresUsed = await _context.PartnerStores.Where(p=>p.StoreName==partnerStore.StoreName).FirstOrDefaultAsync();
            if (partnerStoresUsed != null)
            {
                return BadRequest(new GlobalResponseNoDataDto{
                    success = true,
                    message= "StoreName is used"
                });
            }
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var nameIdentifierClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim == null|nameIdentifierClaim.Value==null) {
                return Unauthorized(new GlobalResponseNoDataDto
                {
                    success=false,
                    message= "Not valid Token"
                });
            }
            var ImgNationalID = await _uploadFileService.UploadFile(partnerStore.ImgNationalID);
            var imgStore = await _uploadFileService.UploadFile(partnerStore.ImgStore,true);
            var EmgComOrFreeRegister = await _uploadFileService.UploadFile(partnerStore.EmgComOrFreeRegister);
            var newPartnerStore = await _context.PartnerStores.AddAsync(new Models.PartnerStore
            {
                Id = Guid.NewGuid(),
                FirstName = partnerStore.FirstName,
                FamilyName = partnerStore.FamilyName,
                IDNumber = partnerStore.IDNumber,
                ImgNationalID = ImgNationalID,
                DateStartComOrFreeRegister = DateOnly.Parse(partnerStore.DateStartComOrFreeRegister),
                DateEndComOrFreeRegister = DateOnly.Parse(partnerStore.DateEndComOrFreeRegister),
                DeliveryType = partnerStore.DeliveryType.Select(e=>(int)e).ToArray(),
                ImgStore = imgStore,
                EmgComOrFreeRegister = EmgComOrFreeRegister,
                NameComOrFreeRegister = partnerStore.NameComOrFreeRegister,
                NumberComOrFreeRegister = partnerStore.NumperComOrFreeRegister,
                PartnerId = Guid.Parse(nameIdentifierClaim.Value),
                StoreName = partnerStore.StoreName,
                //SubTypeId = partnerStore.SubType,
                TypeId = partnerStore.Type,
                City=partnerStore.City
            });
            await _context.SaveChangesAsync();
            var partnerStores = await _context.PartnerStores.Include(ps => ps.Type)/*.Include(ps=>ps.SubType)*/.Where(ps=>ps.Id==newPartnerStore.Entity.Id).FirstOrDefaultAsync();
            return StatusCode(201, new GlobalResponseDebugDto<Models.PartnerStore, string>
            {
                success = true,
                message = "Cre",
                data = partnerStores,
                debug = "No"


            });
        }

        [HttpGet("store_types")]
        public async Task<IActionResult> GetStoreTypes()
        {
           var PartnerStoreSubTypes=  await _context.PartnerStoreTypes/*.Include(ps=>ps.SubTypes)*/.ToListAsync();
            var result = PartnerStoreSubTypes.Select(ps => new StoreTypeDto
            {
                Id = ps.Id,
                Name = ps.Name,
                //SubTypes = ps.SubTypes.Select(st => new SubTypeDto
                //{
                //    Id = st.Id,
                //    Name = st.Name
                //}).ToList()
            }).ToList();
            _logger.LogInformation("Get all typs");
            return Ok(new GlobalResponseDebugDto<List<StoreTypeDto>,string>
            {
                success= true,
                message= "All store types",
               data= result,
               debug="No debug data"
            });
        }

        [HttpPost("verify_otp")]
        public async Task<IActionResult> VerifyOTP([FromBody] Dtos.OTPDto otp)
        {
            var findOTP = await _context.OTPs.Where(o=>o.MobileNumber==otp.MobileNumber&&o.Otp == otp.Otp).FirstOrDefaultAsync();
            if (findOTP == null)
            {
                return BadRequest(new GlobalResponseNoDataDto { success = false, message = "Not Valed otp" });

            }
            var partner = await _context.Partners.Where(p => p.PhoneNumber == otp.MobileNumber).FirstOrDefaultAsync();
            if (partner == null)
            {
                return BadRequest(new GlobalResponseNoDataDto { success = false, message = "Not Valed otp" });

            }
            partner.PhoneNumberVerifed = true;
            partner.PhoneNumber = otp.MobileNumber;
            await _context.SaveChangesAsync();
            var token = _jwtServicePartner.creatJwtToken(partner);
            return Ok(new GlobalResponseDebugDto<LoginBodyDto, string>
            {
                success = true,
                message = "otp valed success",
                data = new LoginBodyDto
                {
                    Partner = new PartnerBodyResDto
                    {
                        Email = partner.Email,
                        Id = partner.Id,
                        PhoneNumper = partner.PhoneNumber,
                    },
                    Token = token
                },
                debug = "No debug Data"
            });
        }

        [HttpPost("send_otp")]
        public async Task<IActionResult> SendOTP([FromBody] Dtos.PartnerSendOtp partnerSendOtp)
        {
          var partnerFound =  await _context.Partners.Where(p=>p.PhoneNumber==partnerSendOtp.PhoneNumper).FirstOrDefaultAsync();
            if (partnerFound == null)
            {
                return BadRequest(new GlobalResponseNoDataDto
                {
                    success=false,
                    message= "Phone numper not valid"
                });
            }
            Random rnd = new Random();
            int otp = rnd.Next(100, 999);
          await   _context.OTPs.AddAsync(new Models.OTP
            {
                Id=Guid.NewGuid(),
                MobileNumber=partnerSendOtp.PhoneNumper,
                Otp=otp.ToString()
            });
          await  _context.SaveChangesAsync();
            return Ok(new GlobalResponseNoBodyDebugDto<OTPDto>
            {
                success=true,
                message="Send otp to your phone",
                debug=new OTPDto
                {
                    MobileNumber=partnerSendOtp.PhoneNumper,
                    Otp=otp.ToString()
                }
            });
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("send_otp_update_phone")]
        public async Task<IActionResult> SendOTPUpdatePhone([FromBody] Dtos.PartnerSendOtp partnerSendOtp)
        {
            var partnerFound = await _context.Partners.Where(p => p.PhoneNumber == partnerSendOtp.PhoneNumper).FirstOrDefaultAsync();
            if (partnerFound != null)
            {
                return BadRequest(new GlobalResponseNoDataDto
                {
                    success = false,
                    message = "Phone numper not valid"
                });
            }
            Random rnd = new Random();
            int otp = rnd.Next(100, 999);
            await _context.OTPs.AddAsync(new Models.OTP
            {
                Id = Guid.NewGuid(),
                MobileNumber = partnerSendOtp.PhoneNumper,
                Otp = otp.ToString()
            });
            await _context.SaveChangesAsync();
            return Ok(new GlobalResponseNoBodyDebugDto<OTPDto>
            {
                success = true,
                message = "Send otp to your phone",
                debug = new OTPDto
                {
                    MobileNumber = partnerSendOtp.PhoneNumper,
                    Otp = otp.ToString()
                }
            });
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("verify_otp_update_phone")]
        public async Task<IActionResult> VerifyOTPUpdatePhone([FromBody] Dtos.OTPDto otp)
        {
            var NameIdentifier =  HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (NameIdentifier == null)
            {
                return Unauthorized();
            }
            var userId = Guid.Parse(NameIdentifier.Value);
            var findOTP = await _context.OTPs.Where(o => o.MobileNumber == otp.MobileNumber && o.Otp == otp.Otp).FirstOrDefaultAsync();
            if (findOTP == null)
            {
                return BadRequest(new GlobalResponseNoDataDto { success = false, message = "Not Valed otp" });

            }
            var partner = await _context.Partners.Where(p => p.Id == userId).FirstOrDefaultAsync();
            if (partner == null)
            {
                return BadRequest(new GlobalResponseNoDataDto { success = false, message = "Not Valed otp" });

            }
            partner.PhoneNumberVerifed = true;
            partner.PhoneNumber = otp.MobileNumber;
            await _context.SaveChangesAsync();
            var token = _jwtServicePartner.creatJwtToken(partner);
            return Ok(new GlobalResponseDebugDto<LoginBodyDto, string>
            {
                success = true,
                message = "otp valed success",
                data = new LoginBodyDto
                {
                    Partner = new PartnerBodyResDto
                    {
                        Email = partner.Email,
                        Id = partner.Id,
                        PhoneNumper = partner.PhoneNumber,
                    },
                    Token = token
                },
                debug = "No debug Data"
            });
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost("reset_password")]
        public async Task<IActionResult> ResetPasswoed([FromBody] PartnerResetPasswoed  partnerResetPasswoed)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var nameIdentifierClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim == null | nameIdentifierClaim.Value == null)
            {
                return Unauthorized(new GlobalResponseNoDataDto
                {
                    success = false,
                    message = "Not valid Token"
                });
            }
            var userId = Guid.Parse(nameIdentifierClaim.Value);

          var userFind =  await _context.Partners.Where(q => q.Id == userId).FirstOrDefaultAsync();
            if(userFind is null)
            {
                return Unauthorized(new GlobalResponseNoDataDto
                {
                    success = false,
                    message = "Not valid Token"
                });
            }
            userFind.Password = _HashPasswordService.HashPassword(partnerResetPasswoed.Password);
           await _context.SaveChangesAsync();

            return Ok(new GlobalResponseNoDataDto
            {
                success=true,
                message="update password sucsses"
            });

        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("img_national_id")]
        public async Task<IActionResult> ImgNationalID()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var nameIdentifierClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Guid userId = Guid.Parse(nameIdentifierClaim.Value);
           var partner= await _context.Partners.Include(p=>p.PartnerStore).FirstOrDefaultAsync(p=>p.Id==userId);

            string _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            var filePath = Path.Combine(_imageFolderPath, partner.PartnerStore[0].ImgNationalID);

            var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var contentType = "image/jpeg"; // Set the appropriate content typeimage

            return File(imageFileStream, contentType);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("img_com_or_free_register")]
        public async Task<IActionResult> ImgComOrFreeRegister()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var nameIdentifierClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Guid userId = Guid.Parse(nameIdentifierClaim.Value);
           var partner= await _context.Partners.Include(p=>p.PartnerStore).FirstOrDefaultAsync(p=>p.Id==userId);

            //string _imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            //var filePath = Path.Combine(_imageFolderPath, partner.PartnerStore[0].EmgComOrFreeRegister);

            //var imageFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            //var contentType = "image/jpeg"; // Set the appropriate content typeimage
           var psthTemp = _tempImgService.Upload(partner.PartnerStore[0].EmgComOrFreeRegister);
            return Ok(psthTemp);
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPatch("update_email")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmail updateEmail)
        {
            
            var nameIdentifierClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim == null)
            {
                return Unauthorized();
            }
           Guid userId = Guid.Parse(nameIdentifierClaim.Value);
            var EmailFound = await _context.Partners.Where(p=>p.Email==updateEmail.Email).FirstOrDefaultAsync();
            if (EmailFound != null) { 
            return BadRequest(new GlobalResponseNoDataDto
            {
                success = false,
                message = "email is used"
            });
            }
           var partnerFound = await _context.Partners.Where(p => p.Id == userId).FirstOrDefaultAsync();
            partnerFound.Email=updateEmail.Email;
            await _context.SaveChangesAsync();

            return Ok(new GlobalResponseNoDataDto
            {
                success=true,
                message="updated email"
            });
            
        }

    }
}
