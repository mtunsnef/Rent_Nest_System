using Azure;
using Microsoft.AspNetCore.Mvc;
using RentNest.Core.DTO;
using RentNest.Service.Interfaces;

namespace RentNest.Web.Controllers
{
    [ApiController]
    [Route("api/chatbot")]
    public class ChatBotController : ControllerBase
    {
        private readonly IAzureOpenAIService _aiService;

        public ChatBotController(IAzureOpenAIService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskAI([FromBody] ChatRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest("Message is required");

            var response = await _aiService.ChatWithAIAsync(request.Message);
            return Ok(new { reply = response });
        }
    }


}
