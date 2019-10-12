using System;
using Microsoft.AspNetCore.Authentication;
using TestEssentials.ToolKit.Authentication;

namespace TestEssentials.ToolKit.Extensions
{
    public static class TestAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTestServerAuthentication(this AuthenticationBuilder builder)
        {
            return builder
                .AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>("Test Scheme", "Test Server Authentication",
                    (o) => { });
        }

        public static AuthenticationBuilder AddTestServerAuthentication(this AuthenticationBuilder builder, Action<TestAuthenticationOptions> configureOptions)
        {
            return builder
                .AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>("Test Scheme", "Test Server Authentication", configureOptions);
        }

    }
}
