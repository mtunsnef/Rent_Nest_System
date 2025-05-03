using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.Configs
{
    public static class AuthSettings
    {
        //Google
        public static readonly string GoogleClientId = Environment.GetEnvironmentVariable("Authentication_Google_ClientId")!;
        public static readonly string GoogleClientSecret = Environment.GetEnvironmentVariable("Authentication_Google_ClientSecret")!;

        //Facebook
        public static readonly string FacebookAppId = Environment.GetEnvironmentVariable("Authentication_Facebook_AppId")!;
        public static readonly string FacebookAppSecret = Environment.GetEnvironmentVariable("Authentication_Facebook_AppSecret")!;
    }
}
