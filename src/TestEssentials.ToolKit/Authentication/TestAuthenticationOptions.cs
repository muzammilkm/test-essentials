using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;

namespace TestEssentials.ToolKit.Authentication
{
    public class TestAuthenticationOptions : AuthenticationSchemeOptions
    {
        public TestAuthenticationOptions()
        {
            ClaimKeys = new List<string>();
        }

        public IEnumerable<string> ClaimKeys { get; set; }
    }
}
