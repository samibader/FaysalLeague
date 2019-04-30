using System.Threading.Tasks;
using FaisalLeague.Api.Models.Login;
using FaisalLeague.Api.Models.Users;
using FaisalLeague.Data.Model;
using FaisalLeague.Queries.Models;

namespace FaisalLeague.Queries
{
    public interface ILoginQueryProcessor
    {
        UserWithToken Authenticate(string username, string password);
        Task<User> Register(RegisterModel model);
        Task ChangePassword(ChangeUserPasswordModel requestModel);
        bool UsernameIsAvailable(string username);
    }
}