using RentNest.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Service.Interfaces
{
    public interface IAzureOpenAIService
    {
        Task<string> GenerateDataPost(PostDataAIDto model);
        Task<string> ChatWithAIAsync(string userMessage);
    }
}
