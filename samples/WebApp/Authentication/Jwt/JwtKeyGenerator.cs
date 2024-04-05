using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApp.Authentication.Jwt
{
    public static class JwtKeyGenerator
    {
        public static SymmetricSecurityKey Generate(string? secret)
        {
#if (NET8_0_OR_GREATER)
            ArgumentException.ThrowIfNullOrWhiteSpace(secret);
#else
            ArgumentNullException.ThrowIfNull(secret);
#endif

            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }
    }
}