using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Models.Users;
using FaisalLeague.Data.Model;
using Microsoft.AspNetCore.Http;

namespace FaisalLeague.Queries
{
    public interface IUsersQueryProcessor
    {
        IQueryable<User> Get();
        User Get(int id);
        Task<User> Create(CreateUserModel model);
        Task<User> Update(int id, UpdateUserModel model);
        Task Delete(int id);
        Task ChangePassword(int id, ChangeUserPasswordModel model);
        Task<User> SetUserImage(int id, string image);
        
    }
}