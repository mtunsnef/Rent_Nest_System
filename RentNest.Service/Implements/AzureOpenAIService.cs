using Azure.AI.OpenAI;
using Azure;
using OpenAI.Chat;
using RentNest.Core.Configs;
using RentNest.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RentNest.Core.DTO;
using RentNest.Common.UtilHelper;
using RentNest.Core.Consts;

namespace RentNest.Service.Implements
{
    public class AzureOpenAIService : IAzureOpenAIService
    {

        public async Task<string> GenerateDataPostAsync(PostDataAIDto model)
        {
            var jsonData = JsonSerializer.Serialize(model);
            var style = model.Style;

            var promptTemplate = PromptAI.Prompt;
            var promptText = promptTemplate.Replace("{json_data}", jsonData)
                                           .Replace("{style}", style);

            var messages = new List<ChatMessage>
        {
            new SystemChatMessage(promptText)
        };

            var client = new AzureOpenAIClient(
                new Uri(AzureOpenAISettings.Endpoint),
                new AzureKeyCredential(AzureOpenAISettings.ApiKey));
            var chatClient = client.GetChatClient(AzureOpenAISettings.DeploymentName);

            var response = await chatClient.CompleteChatAsync(messages);
            var result = response.Value.Content[0].Text;

            return result;
        }
    }
}