using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TestEssentials.ToolKit.Authentication
{
    public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
    {
        private readonly ILogger _logger;
        private readonly ClaimsIdentity _claimsIdentity;

        public TestAuthenticationHandler(IOptionsMonitor<TestAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            _logger = logger.CreateLogger("TestAuth");
            _claimsIdentity = new ClaimsIdentity(new List<Claim>(), "test");
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                _logger.LogDebug("No Bearer Token");
                return Task.FromResult(AuthenticateResult.Fail("No Bearer Token"));
            }

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer", "");
            _logger.LogDebug("Token is available.");

            var tokenDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(token);
            var authenticationTicket = new AuthenticationTicket(
                new ClaimsPrincipal(_claimsIdentity), new AuthenticationProperties(), "Test Scheme");

            _logger.LogDebug("Adding Claims");

            if (Options.ClaimKeys == null || !Options.ClaimKeys.Any())
            {
                Options.ClaimKeys = tokenDic.Keys.ToList();
            }

            foreach (var key in Options.ClaimKeys)
            {
                if (tokenDic.ContainsKey(key))
                {
                    _claimsIdentity.AddClaim(new Claim(key, tokenDic[key].ToString()));
                }
            }

            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }
    }
}
