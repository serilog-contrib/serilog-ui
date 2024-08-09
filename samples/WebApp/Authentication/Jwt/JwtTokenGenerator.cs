using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace WebApp.Authentication.Jwt
{
    public class JwtTokenGenerator(IConfiguration configuration)
    {
        private const string ValidUser = "John Doe";

        private readonly IConfiguration _configuration = configuration;

        public string GenerateJwtToken()
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, $"{Guid.NewGuid()}"),
                new(ClaimTypes.Name, ValidUser),
                new(ClaimTypes.NameIdentifier, ValidUser),
                new(ClaimTypes.UserData, $"{Guid.NewGuid()}"),
                new("example", "my-value") // note: used by the sample policy authorization
            };

            var key = JwtKeyGenerator.Generate(_configuration["Jwt:SecretKey"]);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateInvalidJwtToken()
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, $"{Guid.NewGuid()}"),
                new(ClaimTypes.Name, string.Empty),
                new(ClaimTypes.NameIdentifier, string.Empty),
                new(ClaimTypes.UserData, $"{Guid.NewGuid()}"),
            };

            var key = JwtKeyGenerator.Generate(_configuration["FailingJwt:SecretKey"]);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["FailingJwt:ExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["FailingJwt:Issuer"],
                _configuration["FailingJwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}