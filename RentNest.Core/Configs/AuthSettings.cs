using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentNest.Core.Configs
{
    public class AuthSettings
    {
        public GoogleAuth Google { get; set; } = new();
        public FacebookAuth Facebook { get; set; } = new();

        public class GoogleAuth
        {
            public string ClientId { get; set; } = default!;
            public string ClientSecret { get; set; } = default!;
        }

        public class FacebookAuth
        {
            public string AppId { get; set; } = default!;
            public string AppSecret { get; set; } = default!;
        }
    }
}
