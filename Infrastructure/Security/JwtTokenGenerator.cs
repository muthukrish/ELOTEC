namespace ELOTEC.Infrastructure.Security
{
    using Microsoft.Extensions.Options;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        /// <summary>
        /// The JWT options
        /// </summary>
        private readonly JwtIssuerOptions jwtOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="JwtTokenGenerator"/> class.
        /// </summary>
        /// <param name="jwtOptions">The JWT options.</param>
        public JwtTokenGenerator(IOptions<JwtIssuerOptions> jwtOptions)
        {
            this.jwtOptions = jwtOptions.Value;
        }

        /// <summary>
        /// Creates the token.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public async Task<string> CreateToken(string userName)
        {
            try
            {


                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, await jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(jwtOptions.IssuedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

                var token = new JwtSecurityToken(
                    jwtOptions.Issuer,
                    jwtOptions.Audience,
                    claims,
                    jwtOptions.NotBefore,
                    jwtOptions.Expiration,
                    jwtOptions.SigningCredentials);

                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(token);

                return encodedJwt;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
