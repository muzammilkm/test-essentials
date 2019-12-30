using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

namespace TestEssentials.ToolKit.Authentication.JwtBearer
{
    public class TestJwtBearerAuthenticationOptions : AuthenticationSchemeOptions
    {
        public TestJwtBearerAuthenticationOptions()
        {
            ClaimKeys = new List<string>();
        }

        public IEnumerable<string> ClaimKeys { get; set; }
    }
}
