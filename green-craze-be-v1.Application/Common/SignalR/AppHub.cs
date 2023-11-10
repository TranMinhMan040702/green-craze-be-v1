using green_craze_be_v1.Application.Common.Exceptions;
using green_craze_be_v1.Application.Intefaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace green_craze_be_v1.Application.Common.SignalR
{
    [Authorize]
    public class AppHub : Hub
    {
        private readonly IJwtService _jwtService;

        public AppHub(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        public async Task JoinHub()
        {
            var accessToken = Context.GetHttpContext().Request.Query["access_token"];
            var userPrincipal = _jwtService.ValidateExpiredJWT(accessToken)
                ?? throw new Exception("Invalid token");

            var userId = (userPrincipal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value)
                ?? throw new NotFoundException("User not found");

            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            await Groups.AddToGroupAsync(Context.ConnectionId, Group.SALES);
        }
    }
}