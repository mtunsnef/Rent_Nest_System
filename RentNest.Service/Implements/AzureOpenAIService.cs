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
    }
}