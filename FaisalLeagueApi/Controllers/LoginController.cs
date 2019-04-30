using System.Threading.Tasks;
using System.Linq;
using FaisalLeague.Api.Models.Login;
using FaisalLeague.Api.Models.Users;
using FaisalLeagueApi.Filters;
using FaisalLeagueApi.Maps;
using FaisalLeague.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FaisalLeague.Data.Access.Constants;
using Microsoft.AspNetCore.Http;
using FaisalLeague.Api.Common.Exceptions;
using FaisalLeagueApi.Helpers;
using FaisalLeague.Security;
using AutoMapper;

namespace FaisalLeague.Server.RestAPI
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginQueryProcessor _query;
        private readonly IUsersQueryProcessor _userQuery;
        private readonly IMapper _mapper;
        private readonly IImageHandler _imageHandler;
        private readonly ISecurityContext _context;

        public LoginController(ILoginQueryProcessor query, IMapper mapper, IImageHandler imageHandler, ISecurityContext context, IUsersQueryProcessor userQuery)
        {
            _query = query;
            _mapper = mapper;
            _imageHandler = imageHandler;
            _context = context;
            _userQuery = userQuery;
        }

        [HttpPost("Authenticate")]
        [ValidateModel]
        public UserWithTokenModel Authenticate([FromBody] LoginModel model)
        {
            var result = _query.Authenticate(model.Username, model.Password);

            var resultModel = _mapper.Map<UserWithTokenModel>(result);

            return resultModel;
        }

        [HttpPost("Register")]
        [ValidateModel]
        public async Task<UserModel> Register([FromBody] RegisterModel model)
        {
            var result = await _query.Register(model);
            var resultModel = _mapper.Map<UserModel>(result);
            return resultModel;
        }

        [HttpPost("Password")]
        [ValidateModel]
        //[Authorize(Roles = Roles.AdministratorOrManager)]
        [Authorize]
        public async Task ChangePassword([FromBody]ChangeUserPasswordModel requestModel)
        {
            await _query.ChangePassword(requestModel);
        }

        
    }
}