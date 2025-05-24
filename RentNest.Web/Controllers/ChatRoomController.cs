using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;
using RentNest.Core.Consts;
using RentNest.Core.Domains;
using RentNest.Core.UtilHelper;
using RentNest.Service.Interfaces;
using RentNest.Web.Hubs;
using RentNest.Web.Models;
using System.Security.Claims;

namespace RentNest.Web.Controllers
{
    [Authorize(AuthenticationSchemes = AuthSchemes.Cookie, Roles = $"{UserRoles.User},{UserRoles.Landlord}")]
    public class ChatRoomController : Controller
    {
        private readonly IConversationService _conversationService;

        public ChatRoomController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [Route("tro-chuyen")]
        public async Task<IActionResult> Index()
        {
            int? userId = User.GetUserId();

            ViewData["CurrentUserId"] = userId;

            var conversations = await _conversationService.GetByUserIdAsync(userId.Value);
            return View(conversations);
        }

        [HttpGet]
        [Route("api/v1/chatroom/detail/{id}")]
        public async Task<IActionResult> DetailMessage(int id)
        {
            var currentUserId = User.GetUserId();
            var conversation = await _conversationService.GetConversationWithMessagesAsync(id);

            if (conversation == null) return NotFound();

            var otherUser = conversation.SenderId == currentUserId ? conversation.Receiver : conversation.Sender;

            var vm = new ChatConversationViewModel
            {
                ConversationId = conversation.ConversationId,
                ReceiverId = otherUser?.AccountId ?? 0,
                ReceiverFullName = otherUser?.UserProfile?.FirstName + " " + otherUser?.UserProfile?.LastName ?? "Không xác định",
                ReceiverAvatarUrl = otherUser?.UserProfile?.AvatarUrl ?? "/images/person_1.jpg",
                LastActiveAt = otherUser.LastActiveAt,

                PostId = conversation.PostId,
                PostTitle = conversation.Post?.Title,
                PostImageUrl = conversation.Post?.Accommodation?.AccommodationImages?.FirstOrDefault()?.ImageUrl ?? "/images/work-1.jpg",
                PostPrice = conversation.Post?.Accommodation?.Price,

                Messages = conversation.Messages.Select(m => new MessageViewModel
                {
                    SenderId = m.SenderId,
                    Content = m.Content,
                    SentAt = m.SentAt
                }).ToList()
            };

            return Json(vm);
        }
    }
}
