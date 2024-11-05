using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using momken_backend.Data;
using momken_backend.Dtos;

namespace momken_backend.Controllers
{
    [Route("api/v1/admin/partner")]
    [ApiController]
    public class AdminPartner(AppDbContext _context) : ControllerBase
    {

        [HttpPost("add_store_type")]
        public async Task<IActionResult> AddStoreType([FromBody] AddPartnerStoreTypeDto addPartnerStoreTypeDto)
        {
            foreach(string typeString in addPartnerStoreTypeDto.types)
            {
                if (string.IsNullOrEmpty(typeString)) return BadRequest("not valed data");
            await _context.PartnerStoreTypes.AddAsync(new Models.PartnerStoreTypeCategories
            {
                Id=Guid.NewGuid(),
                Name=typeString,
            });
            }
            await _context.SaveChangesAsync();
            var all = await _context.PartnerStoreTypes.ToListAsync();
            return Ok(all);

        }

    }
}
