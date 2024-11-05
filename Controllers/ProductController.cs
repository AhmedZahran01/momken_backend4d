using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using momken_backend.Data;
using momken_backend.Dtos;
using momken_backend.Dtos.DataRespons;
using momken_backend.Services;
using System.Security.Claims;

namespace momken_backend.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(AppDbContext _context,IUploadFileService _uploadFileService) : ControllerBase
    {

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] Dtos.ProductDto productDto)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var nameIdentifierClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Guid userId = Guid.Parse(nameIdentifierClaim.Value);
            var mineImg = await _uploadFileService.UploadFile(productDto.MineImg,true);
            var moreImgs = new List<string>();
            if(productDto.MoreImgs != null)
            {
            foreach (var m in productDto.MoreImgs) {
                var img = await _uploadFileService.UploadFile(m,true);
                moreImgs.Add(img);
            }

            }

         var prodact =   await   _context.Products.AddAsync(new Models.Product
            {
                Name = productDto.Name,
                Allergens = productDto.Allergens,
                 Calories = productDto.Calories,
                 Description = productDto.Description,
                 MineImg = mineImg,
                 TypeId = productDto.Type,
                 //SubTypeId= productDto.SubType,
                 MoreImgs = moreImgs.ToArray(),
                 Price = productDto.Price,
                 partnerId=userId
            });
            var typeName = await _context.PartnerStoreTypes.Where(t => t.Id == productDto.Type).FirstOrDefaultAsync();
            await _context.SaveChangesAsync();
            return Ok(new GlobalResponseDebugDto<Dtos.DataRespons.ProductCreated, string>
            {
                success = true,
                message = "Created prodact success",
                data = new ProductCreated
                {
                    Id = prodact.Entity.Id,
                    Allergens = prodact.Entity.Allergens,
                    Calories = prodact.Entity.Calories,
                    Name = prodact.Entity.Name,
                    Type= typeName?.Name??"",
                    TypeId= prodact.Entity.TypeId,
                    Description = prodact.Entity.Description,
                    MineImg = "public/" + mineImg,
                    MoreImgs = prodact.Entity.MoreImgs.Select(i => "public/" + i).ToArray(),
                    Price=prodact.Entity.Price

                },
                debug=""
            });

        }
        [HttpGet("all")]
        public async Task<IActionResult> All()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var nameIdentifierClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Guid userId = Guid.Parse(nameIdentifierClaim.Value);

         var allProdacts = await _context.Products.Where(p=>p.partnerId==userId&&p.deletedtAt==null).Select(p=>new ProductCreated
            {
                Allergens=p.Allergens,
                Name = p.Name,
                Calories=p.Calories,
                Id =p.Id,
                Price=p.Price,
                Description=p.Description,
                MineImg= "public/" + p.MineImg,
                Type= p.Type.Name,
                TypeId= p.TypeId,
                MoreImgs = p.MoreImgs.Select(i => "public/" + i).ToArray(),
            }).ToListAsync();
            return Ok(new GlobalResponseDebugDto<List<ProductCreated>, string>
            {
                success = true,
                message ="All prodacts",
                data = allProdacts,
                debug=""
            });
        }
        [HttpDelete("soft_delete/{id}")]
        public async Task<IActionResult> SoftDelete(Guid id)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var nameIdentifierClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Guid userId = Guid.Parse(nameIdentifierClaim.Value);
           var prodact = await _context.Products.Where(p => p.Id == id && p.partnerId == userId&&p.deletedtAt==null).FirstOrDefaultAsync();

            if (prodact == null)
            {
                return NotFound(new GlobalResponseNoDataDto
                {
                    success = false,
                    message = "Not Found"
                });
            }
            prodact.deletedtAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return Ok(new GlobalResponseNoDataDto
            {
                success=true,
                message= "soft delete success"
            });

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(Guid id)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            var nameIdentifierClaim = claimsIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            Guid userId = Guid.Parse(nameIdentifierClaim.Value);
            var prodact = await _context.Products.Where(p => p.Id == id && p.partnerId == userId && p.deletedtAt == null).Select(p => new ProductCreated
            {
                Allergens = p.Allergens,
                Calories = p.Calories,
                Description = p.Description,
                Id = p.Id,
                Type = p.Type.Name,
                TypeId=p.TypeId,
                MineImg = "public/" + p.MineImg,
                MoreImgs = p.MoreImgs.Select(i => "public/" + i).ToArray(),
                Name = p.Name,
                Price = p.Price
            }).FirstOrDefaultAsync();

            if(prodact == null)
            {
                return NotFound(new GlobalResponseNoDataDto
                {
                    success = false,
                    message = "Not Found"
                });
            }
            return Ok(new GlobalResponseDebugDto<ProductCreated, string>
            {
                success = true,
                message = "Prodact",
                data = prodact
            });

        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update(Guid id,[FromForm] Dtos.ProductUpdateDto productUpdateDto)
        {
            var nameIdentifierClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim == null)
            {
                return Unauthorized();
            }
            Guid userId = Guid.Parse(nameIdentifierClaim.Value);
           var prodactFound = await _context.Products.Where(p=>p.Id==id&&p.partnerId==userId&&p.deletedtAt==null).FirstOrDefaultAsync();
            if (prodactFound == null) { 
                return NotFound(new GlobalResponseNoDataDto
                {
                    success=false,
                    message ="Not Found"
                });
            }
            string? mineImg =null ;
            if (productUpdateDto.MineImg!=null)
            {
             mineImg = await _uploadFileService.UploadFile(productUpdateDto.MineImg, true);
                if (!prodactFound.MineImg.IsNullOrEmpty())
                {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "Public", prodactFound.MineImg);
                    var fileexist = System.IO.File.Exists(filePath);
                if (fileexist)
                {
                        System.IO.File.Delete(filePath);
                        }

                }
            }
            var moreImgs = new List<string>(prodactFound.MoreImgs);
            if (productUpdateDto.MoreImgs != null)
            {
                foreach (var m in productUpdateDto.MoreImgs)
                {
                    var img = await _uploadFileService.UploadFile(m, true);
                    moreImgs.Add(img);
                }

            }
            prodactFound.Price = productUpdateDto.Price?? prodactFound.Price;
            prodactFound.Name= productUpdateDto.Name??prodactFound.Name;
            prodactFound.Description = productUpdateDto.Description ?? prodactFound.Description;
            prodactFound.Calories = productUpdateDto.Calories?? prodactFound.Calories;
            prodactFound.Allergens=productUpdateDto.Allergens??prodactFound.Allergens;
            prodactFound.MineImg = mineImg??prodactFound.MineImg;
            prodactFound.MoreImgs = moreImgs.IsNullOrEmpty()? prodactFound.MoreImgs:moreImgs.ToArray();
            prodactFound.TypeId = productUpdateDto.Type ?? prodactFound.TypeId;
            prodactFound.updatAt = DateTime.UtcNow;
            var type = _context.PartnerStoreTypes.FirstOrDefault(t=>t.Id == prodactFound.TypeId);
            await _context.SaveChangesAsync();

            return Ok(new GlobalResponseDebugDto<Dtos.DataRespons.ProductCreated, string>
            {
                success = true,
                message = "Created prodact success",
                data = new ProductCreated
                {
                    Id = prodactFound.Id,
                    Allergens = prodactFound.Allergens,
                    Calories = prodactFound.Calories,
                    Name = prodactFound.Name,
                    Type = type?.Name ?? "",
                    TypeId= type?.Id,
                    Description = prodactFound.Description,
                    MineImg = "public/" + mineImg,
                    MoreImgs = prodactFound.MoreImgs.Select(i => "public/" + i).ToArray(),
                    Price = prodactFound.Price

                },
                debug = ""
            });


        }

       



    }
}
