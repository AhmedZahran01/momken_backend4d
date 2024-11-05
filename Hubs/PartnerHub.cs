using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using momken_backend.Data;
using momken_backend.Dtos.DataRespons;
using momken_backend.Models;
using SignalRSwaggerGen.Attributes;
using System.Security.Claims;

namespace momken_backend.Hubs
{
    [SignalRHub("hub/v1/partner_hub")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PartnerHub:Hub<IPartnerHubLisen>
    {
        private readonly ILogger<PartnerHub> _logger;
        private readonly AppDbContext _context;

        public PartnerHub(ILogger<PartnerHub> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
        }
        
        public async Task SendMassge(string message,Guid roomId)
        {
            var nameIdentifierClaim = Context.User.FindFirst(ClaimTypes.NameIdentifier);
            Guid userId = Guid.Parse(nameIdentifierClaim.Value);
            await Clients.All.MessageTest(message);
        var roomFound = await _context.PartnerClientRooms.Where(r=>r.Id == roomId && r.PartnerId == userId).FirstOrDefaultAsync();
         var messageCreated = await _context.PartnerClientRoomMessages.AddAsync(new PartnerClientRoomMessage
            {
                Massage = message,
                UserId = userId,
                UserType = Enums.UeserTypes.Partiner.ToString(),
                RoomId = roomFound.Id,

            });
            await _context.SaveChangesAsync();
           await Clients.User(userId.ToString()).MessagePartnerClientRoomMessage(new PartnerClintMessage
            {
                Id=messageCreated.Entity.Id,
                CreatedAt=messageCreated.Entity.CreatedAt,
                IsYour=true,
                Massage=messageCreated.Entity.Massage,
                RoomId = messageCreated.Entity.RoomId,
                updatAt = messageCreated.Entity.updatAt
            });
            _logger.LogInformation($"send message {message}");
        }
    }
    [SignalRHub("partner_hub_lisen_for")]
    public interface IPartnerHubLisen
    {
        Task MessageTest(string arg2);
        Task MessagePartnerClientRoomMessage(PartnerClintMessage partnerClientRoomMessage);
    }
}
