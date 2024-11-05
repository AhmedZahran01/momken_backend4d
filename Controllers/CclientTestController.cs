using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using momken_backend.Data;
using momken_backend.Dtos;
using momken_backend.Dtos.DataRespons;
using momken_backend.Hubs;

namespace momken_backend.Controllers
{
    [Route("api/v1/clientTest")]
    [ApiController]
    public class CclientTestController(AppDbContext _context, IHubContext<PartnerHub, IPartnerHubLisen> _hubPartner) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Creat([FromBody] ClientDto clientDto)
        {
            
        var newClint = await _context.Clients.AddAsync(new Models.Client
           {
               FirstName=clientDto.FirstName,
               FamilyName= clientDto.FamilyName,

           }
               );
            await _context.SaveChangesAsync();
            return Ok(new GlobalResponseDebugDto<Models.Client, string>
            {
                success=true,
                message="Creat new client for test chat",
                data=newClint.Entity,
                debug="No data for debug"
            });
        }  

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {

            var allClints = await _context.Clients.ToListAsync();
            return Ok(new GlobalResponseDebugDto<List<Models.Client>, string>
            {
                success=true,
                message="get all clints",
                data= allClints,
                debug="No data for debug"
            });
        } 
        
        [HttpPost("send_test_message_for_partner")]
        public async Task<IActionResult> SendTestMessage([FromBody] ClientRoomMessageDto clientRoomMessageDto)
        {

           await _hubPartner.Clients.All.MessageTest("hi");
        var roomFound =   await _context.PartnerClientRooms.Where(r => r.PartnerId == clientRoomMessageDto.PartnerId && r.ClientId == clientRoomMessageDto.ClientId).FirstOrDefaultAsync();
            if (roomFound == null) {
              
          roomFound =   (await _context.PartnerClientRooms.AddAsync(new Models.PartnerClientRoom
            {
             ClientId= clientRoomMessageDto.ClientId,
             PartnerId= clientRoomMessageDto.PartnerId
            })).Entity;
                await _context.SaveChangesAsync();
            }
          var message =  await _context.PartnerClientRoomMessages.AddAsync(new Models.PartnerClientRoomMessage
           {
               RoomId= roomFound.Id ,
               Massage = clientRoomMessageDto.Massage,
               UserId= clientRoomMessageDto.ClientId,
               UserType= Enums.UeserTypes.Client.ToString(),
          });
            await _context.SaveChangesAsync();
            await   _hubPartner.Clients.User(clientRoomMessageDto.PartnerId.ToString()).MessagePartnerClientRoomMessage(new PartnerClintMessage
            {
                Id=message.Entity.Id,
                Massage=message.Entity.Massage,
                RoomId=message.Entity.RoomId,
                IsYour=false,
                CreatedAt=message.Entity.CreatedAt,
                updatAt=message.Entity.updatAt
            });
            return Ok(new GlobalResponseNoDataDto
            {
                success=true,
                message="send for all"
            });
        }
    }
}
