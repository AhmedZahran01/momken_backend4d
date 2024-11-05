using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using momken_backend.Data;
using momken_backend.Dtos;
using momken_backend.Dtos.DataRespons;
using momken_backend.Dtos.ViewsDto;
using momken_backend.Services;
using System.Security.Claims;

namespace momken_backend.Controllers
{
    [Route("api/v1/partners")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PartnerController(AppDbContext _context, PdfService _pdfService, IUploadFileService _uploadFileService) : ControllerBase
{


        [HttpGet("message_room_clint/{id}")]
        public async Task<IActionResult> GetAllMessageFromRoomClint(Guid id)
        {
            var nameIdentifierClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim == null)
            {
                return Unauthorized();
            }
            Guid userId = Guid.Parse(nameIdentifierClaim.Value);
            var room = await _context.PartnerClientRooms.Where(r => r.PartnerId == userId&&r.Id == id).FirstAsync();
            if (room == null)
            {
                return Unauthorized();
            }
            var messages = await _context.PartnerClientRoomMessages.Where(r => r.RoomId == room.Id).Select(m=>new PartnerClintMessage
            {
                CreatedAt=m.CreatedAt,
                Id=m.Id ,
                IsYour= (m.UserType==Enums.UeserTypes.Partiner.ToString())&&(m.UserId==userId),
                Massage=m.Massage??"",
                updatAt=m.updatAt,
                RoomId=m.RoomId
            }).OrderBy(m=>m.CreatedAt).ToListAsync();
            var messagesAll = await _context.PartnerClientRoomMessages
    .Where(m => m.RoomId == room.Id&&m.UserType== Enums.UeserTypes.Client.ToString()&&!m.IsRecipientShow)
    .ToListAsync();
            foreach (var m in messagesAll)
            {
                m.IsRecipientShow = true;
               
            }
            await _context.SaveChangesAsync();
            return Ok(new GlobalResponseDebugDto<List<PartnerClintMessage>, string> { 
            success=true,
             message="All messgage",
             data= messages
            });


        }

        [HttpGet("all_room_clint")]
        public async Task<IActionResult> GetAllRoomClint()
        {
            var nameIdentifierClaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if (nameIdentifierClaim == null)
            {
                return Unauthorized();
            }
            Guid userId = Guid.Parse(nameIdentifierClaim.Value);

            var allRooms = await _context.PartnerClientRooms.Where(r => r.PartnerId == userId).Include(r=>r.Client).Select(r=>new PartnerClientRoomWithClint
            {
                Id=r.Id,
                Client=new PartnerClientRoomWithClintClient
                {
                  FirstName=  r.Client.FirstName,
                  FamilyName=r.Client.FamilyName
                },
                LastMessage = _context.PartnerClientRoomMessages
            .Where(m => m.RoomId == r.Id)
            .OrderByDescending(m => m.CreatedAt) // Assuming CreatedAt is the timestamp of the message
            .Select(m => m.Massage)       // Fetching the message content
            .FirstOrDefault()??"",
                LastMessageDateTime = _context.PartnerClientRoomMessages
            .Where(m => m.RoomId == r.Id)
            .OrderByDescending(m => m.CreatedAt) // Assuming CreatedAt is the timestamp of the message
            .Select(m => m.CreatedAt)       // Fetching the message content
            .FirstOrDefault() 
                ,
                MessagesNotShow= _context.PartnerClientRoomMessages
            .Where(m => m.RoomId == r.Id&& (m.UserType == Enums.UeserTypes.Client.ToString())&&!m.IsRecipientShow)
            .Count()

            }).ToListAsync();
            return Ok(new GlobalResponseDebugDto<List<PartnerClientRoomWithClint>,string>{
                success=true,
                message="All rooms",
                data= allRooms,
                debug=""
            });


        }

        [HttpGet("pdf")]
        public async Task<IActionResult> GetPdf()
        {
            var htmlContent = await _pdfService.RenderViewToStringAsync(ControllerContext,"Pdf", new InvoicePdf
            {
                FirstName="محمد شاكر الاكيلاني"
            });
            var pdf = _pdfService.GeneratePdf(htmlContent);
            return File(pdf, "application/pdf", "document.pdf");
        }
        [HttpPatch("store_update")]
        public async Task<IActionResult> UpdateStore([FromForm] UpdateStoreDto updateStoreDto)
        {
            var userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            var store = await _context.PartnerStores.Where(s => s.PartnerId == userId).FirstAsync();
            if (updateStoreDto.ImgStore != null)
            {
                var path = await _uploadFileService.UploadFile(updateStoreDto.ImgStore, true);
                if (!String.IsNullOrEmpty(store.ImgStore))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "public", store.ImgStore);
                    if (System.IO.File.Exists(oldPath))
                    {

                    System.IO.File.Delete(oldPath);
                    }

                }
                store.ImgStore = path;
            }
            if (updateStoreDto.DeliveryType != null&&updateStoreDto.DeliveryType.Count>0)
            {

                store.DeliveryType = updateStoreDto.DeliveryType.Select(d => (int)d).ToArray();
            }

            await _context.SaveChangesAsync();

            return Ok(new GlobalResponseNoDataDto
            {
                success = true,
                message = "Updated success"
            });
        }
    }

}
