using System;
using Microsoft.AspNetCore.Authentication;

namespace TestEssentials.ToolKit.Authentication.JwtBearer.Extensions
{
    public static class TestAuthenticationExtensions
    {
        public static AuthenticationBuilder AddTestJwtBearerAuthentication(this AuthenticationBuilder builder)
        {
            return builder
                .AddScheme<TestJwtBearerAuthenticationOptions, TestJwtBearerAuthenticationHandler>(JwtTestBearerDefaults.AuthenticationScheme, "Test Server Authentication",
                    (o) => { });
        }

        public static AuthenticationBuilder AddTestJwtBearerAuthentication(this AuthenticationBuilder builder, Action<TestJwtBearerAuthenticationOptions> configureOptions)
        {
            return builder
                .AddScheme<TestJwtBearerAuthenticationOptions, TestJwtBearerAuthenticationHandler>(JwtTestBearerDefaults.AuthenticationScheme, "Test Server Authentication", configureOptions);
        }

    }
}
