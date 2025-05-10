using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.Configs
{
    public static class AzureOpenAISettings
    {
        public static readonly string Endpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT")!;
        public static readonly string DeploymentName = Environment.GetEnvironmentVariable("AZURE_OPENAI_DEPLOYMENT")!;
        public static readonly string ApiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_KEY")!;
    }
}
