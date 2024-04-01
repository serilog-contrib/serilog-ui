using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApp.Authentication.Jwt
{
    public static class JwtKeyGenerator
    {
        public static SymmetricSecurityKey Generate(string? secret)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(secret);

            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }
    }
}