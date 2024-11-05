

using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace momken_backend.Providers
{
    public class ProductUserIdProvider : IUserIdProvider
    {
        string IUserIdProvider.GetUserId(HubConnectionContext connection)
        {
          var userId =  connection.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
          //var  httpContextAuth = httpContext.Request.Query["access_token"];

                return String.IsNullOrEmpty(userId) ? Guid.NewGuid().ToString() : userId.ToString();
        }
    }
}
