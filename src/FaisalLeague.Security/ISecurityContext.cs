using FaisalLeague.Data.Model;

namespace FaisalLeague.Security
{
    public interface ISecurityContext
    {
        User User { get; }

        bool IsAdministrator { get; }
    }
}