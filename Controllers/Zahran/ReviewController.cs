using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using momken_backend.Data;
using momken_backend.Dtos.DataRespons;
using momken_backend.Dtos.Zahran;
using momken_backend.Models;
using System.Security.Claims;

namespace momken_backend.Controllers.Zahran
{
    [Route("api/v1/client/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "client")]
    public class ReviewController : ControllerBase
    {
        #region Constractor Region
 
        private readonly AppDbContext _context;

        public ReviewController(AppDbContext dbContext)
        {
            _context = dbContext;
        }


        #endregion
         

        #region Reviews Region

        [HttpPost("MakeCommentByClient")]
        public async Task<IActionResult> MakeCommentByClient([FromBody] reviewDataDto revsiewData)
        {
            string? StringClientId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid? GuidClientId = Guid.Parse(StringClientId);
            if (GuidClientId is not null)
            { 
                var clientExists = await _context.Clients.Where(c => c.Id == GuidClientId).FirstOrDefaultAsync();
                var partnerStoreExists = await _context.PartnerStores.Where(c => c.Id == revsiewData.partnerStoreId).ToListAsync();

                if (clientExists is not null && partnerStoreExists is not null)
                {
                    var review = new PartnerStoreClientReview()
                    {
                        clientId = GuidClientId,
                        partnerStoreId = revsiewData.partnerStoreId,
                        ReviewMessage = revsiewData.ReviewMessage,
                        EvaluationNumber = revsiewData.evaluationNumber
                    };
                    await _context.ReviewsOfClient.AddAsync(review);
                    await _context.SaveChangesAsync();
                    return Ok(new GlobalResponseDebugDto<reviewDataDto, string>
                    {
                        success = true,
                        message = "Review added successfully.",
                        data = revsiewData,
                        debug = "No data for debug"
                    });
                }

            }
            return BadRequest(new GlobalResponseNoDataDto
            {
                success = false,
                message = "There is An Error for client or Partner Store "
            });
        }

        #endregion

        #region Reviews Region

        [HttpPost("getFinalRate")]
        public async Task<IActionResult> getFinalRate(Guid? PartnerStoreId)
        {
            decimal finalRate = 0;
            decimal Sum = 0;
            if (PartnerStoreId is not null)
            { 
                var RateList = await _context.partnerStoreRates.Where(p => p.partnerStoreId == PartnerStoreId).Select(p => p.partnerStoreRate).ToListAsync();
                foreach (var item in RateList)
                {
                    Sum += item;
                }

                finalRate = Sum / RateList.Count;
            }
            return BadRequest(new GlobalResponseNoDataDto
            {
                success = false,
                message = "You Should Choose a Specific Partner Store "
            });
        }

        #endregion
    }
}
