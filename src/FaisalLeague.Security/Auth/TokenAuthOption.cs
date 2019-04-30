using System;
using Microsoft.IdentityModel.Tokens;

namespace FaisalLeague.Security.Auth
{
    public class TokenAuthOption
    {
        public static string Audience { get; } = "FaisalLeagueAudience";
        public static string Issuer { get; } = "FaisalLeagueIssuer";
        public static RsaSecurityKey Key { get; } = new RsaSecurityKey(RSAKeyHelper.GenerateKey());
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);

        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromDays(1);
        public static string TokenType { get; } = "Bearer"; 
    }
}
