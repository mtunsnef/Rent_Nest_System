using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RentNest.Core.Domains;
using RentNest.Service.Interfaces;
using System.Security.Claims;

namespace RentNest.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IAccountService _accountService;
        public ChatHub(IChatService chatService, IAccountService accountService)
        {
            _chatService = chatService;
            _accountService = accountService;
        }
        public async Task SendMessage(int conversationId, int receiverId, string message)
        {
            var senderId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(senderId, out int parsedSenderId))
                return;

            await _accountService.UpdateLastActiveAsync(parsedSenderId);


            var mess = new Message
            {
                ConversationId = conversationId,
                SenderId = parsedSenderId,
                Content = message
            };
            await _chatService.AddMessage(mess);

            await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", parsedSenderId, message);
        }

        public override async Task OnConnectedAsync()
        {
            var userIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdStr, out int userId))
            {
                await _accountService.SetUserOnlineAsync(userId, true);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userIdStr = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdStr, out int userId))
            {
                await _accountService.SetUserOnlineAsync(userId, false);
                await _accountService.UpdateLastActiveAsync(userId);
            }

            await base.OnDisconnectedAsync(exception);
        }

    }
}
