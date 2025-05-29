using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;
using RentNest.Core.Consts;
using RentNest.Core.Domains;
using RentNest.Core.DTO;
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
        private readonly IQuicklyReplyTemplateService _quicklyReplyTemplateService;
        private readonly IAccountService _accountService;
        public ChatRoomController(IConversationService conversationService, IQuicklyReplyTemplateService quicklyReplyTemplateService, IAccountService accountService)
        {
            _conversationService = conversationService;
            _quicklyReplyTemplateService = quicklyReplyTemplateService;
            _accountService = accountService;
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
            var user = await _accountService.GetAccountById(currentUserId ?? 0);

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
                IsOnline = otherUser.IsOnline,

                PostId = conversation.PostId,
                PostTitle = conversation.Post?.Title,
                PostImageUrl = conversation.Post?.Accommodation?.AccommodationImages?.FirstOrDefault()?.ImageUrl ?? "/images/work-1.jpg",
                PostPrice = conversation.Post?.Accommodation?.Price,

                Messages = conversation.Messages.Select(m => new MessageViewModel
                {
                    SenderId = m.SenderId,
                    ImageUrl = m.ImageUrl,
                    Content = m.Content,
                    SentAt = m.SentAt
                }).ToList(),
                QuickReplies = await _quicklyReplyTemplateService.GetQuickRepliesByRoleAsync(user.Role)
            };

            return Json(vm);
        }

        [HttpPost]
        [Route("/api/v1/quick-messages")]
        public async Task<IActionResult> AddQuickMessage([FromBody] QuickMessDto dto)
        {
            var currentUserId = User.GetUserId();
            var user = await _accountService.GetAccountById(currentUserId ?? 0);

            await _quicklyReplyTemplateService.AddQuickReplyAsync(dto.Content, user.Role, currentUserId ?? 0);
            return Ok(new {message = "Đã thêm tin nhắn nhanh"});
        }
    }
}
