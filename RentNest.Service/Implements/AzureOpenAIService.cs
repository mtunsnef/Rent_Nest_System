using Azure.AI.OpenAI;
using Azure;
using OpenAI.Chat;
using RentNest.Core.Configs;
using RentNest.Service.Interfaces;
using System.Text.Json;
using RentNest.Core.DTO;
using RentNest.Core.Consts;
using Microsoft.Extensions.Options;

namespace RentNest.Service.Implements
{
    public class AzureOpenAIService : IAzureOpenAIService
    {
        private readonly AzureOpenAISettings _setting;

        public AzureOpenAIService(IOptions<AzureOpenAISettings> setting)
        {
            _setting = setting.Value;
        }

        public async Task<string> GenerateDataPost(PostDataAIDto model)
        {
            var jsonData = JsonSerializer.Serialize(model);
            var style = model.AiStyle;

            var promptTemplate = PromptAI.Prompt;
            var promptText = promptTemplate.Replace("{json_data}", jsonData)
                                           .Replace("{style}", style);

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(promptText)
            };

            var client = new AzureOpenAIClient(
                new Uri(_setting.Endpoint),
                new AzureKeyCredential(_setting.ApiKey));
            var chatClient = client.GetChatClient(_setting.DeploymentName);

            var response = await chatClient.CompleteChatAsync(messages);
            var result = response.Value.Content[0].Text;

            return result;
        }

        public async Task<string> ChatWithAIAsync(string userMessage)
        {
            var messages = new List<ChatMessage>
                {
                    new SystemChatMessage(@"
                        Bạn là một trợ lý AI giúp người dùng tìm trọ (dành cho khách hoặc người thuê) hoặc đăng tin (dành cho chủ trọ). 
                        Khi người dùng hỏi về địa điểm hoặc nhu cầu thuê trọ, hãy tìm trong danh sách bài viết đã được cung cấp.
                        Nếu có bài đăng phù hợp, hãy trả lời rõ ràng và **ghi rõ đường dẫn bài đăng**.
                        KHÔNG dùng bất kỳ từ in đậm nào trong câu trả lời.
                    "),
                    new UserChatMessage(userMessage)
                };

            var client = new AzureOpenAIClient(
                         new Uri(_setting.Endpoint),
                         new AzureKeyCredential(_setting.ApiKey));
            var chatClient = client.GetChatClient(_setting.DeploymentName);

            var response = await chatClient.CompleteChatAsync(messages);
            return response.Value.Content[0].Text;
        }

    }
}