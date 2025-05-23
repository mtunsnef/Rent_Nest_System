using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using RentNest.Core.Consts;
using RentNest.Core.Domains;
using RentNest.Service.Interfaces;
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
            var accountId = HttpContext.Session.GetString("AccountId");
            ViewData["CurrentUserId"] = int.Parse(accountId);

            var conversations = await _conversationService.GetBySenderIdAsync(int.Parse(accountId));
            return View(conversations);
        }

        [HttpGet]
        [Route("api/v1/chatroom/detail/{id}")]
        public async Task<IActionResult> DetailMessage(int id)
        {
            var currentUserIdStr = HttpContext.Session.GetString("AccountId");
            if (string.IsNullOrEmpty(currentUserIdStr)) return Unauthorized();
            var currentUserId = int.Parse(currentUserIdStr);
            var conversation = await _conversationService.GetConversationWithMessagesAsync(id);

            if (conversation == null) return NotFound();

            var otherUser = conversation.SenderId == currentUserId ? conversation.Receiver : conversation.Sender;

            var vm = new ChatConversationViewModel
            {
                ConversationId = conversation.ConversationId,
                ReceiverFullName = otherUser?.UserProfile?.FirstName + " " + otherUser?.UserProfile?.LastName ?? "Không xác định",
                ReceiverAvatarUrl = otherUser?.UserProfile?.AvatarUrl ?? "/images/person_1.jpg",
                LastSeenText = "Hoạt động 4 giờ trước", // TODO: Bạn có thể lấy thời gian thực từ DB

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
