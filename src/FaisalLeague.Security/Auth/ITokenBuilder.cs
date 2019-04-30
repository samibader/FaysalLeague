using System;

namespace FaisalLeague.Security.Auth
{
    public interface ITokenBuilder
    {
        string Build(string name, string id, string[] roles, DateTime expireDate);
    }
}