namespace ELOTEC
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using ELOTEC.Infrastructure.Helpers;
    using ELOTEC.Infrastructure.Security;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    public static class StartupExtensions
    {
        public static void AddJwt(this IServiceCollection services)
        {
            services.AddOptions();

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ConfigHelper.AppSettings.Secret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = ConfigHelper.AppSettings.Issuer;
                options.Audience = ConfigHelper.AppSettings.Audience;
                options.SigningCredentials = signingCredentials;
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingCredentials.Key,
                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = ConfigHelper.AppSettings.Issuer,
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = ConfigHelper.AppSettings.Audience,
                // Validate the token expiry
                ValidateLifetime = true,
                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = tokenValidationParameters;
                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = (context) =>
                            {
                                var token = context.HttpContext.Request.Headers["Authorization"];
                                if (token.Count > 0 && token[0].StartsWith("Token ", StringComparison.OrdinalIgnoreCase))
                                {
                                    context.Token = token[0].Substring("Token ".Length).Trim();
                                }

                                return Task.CompletedTask;
                            }
                        };
                    });
        }
    }
}
