﻿using System.Threading.Tasks;
using FaisalLeague.Api.Models.Login;
using FaisalLeague.Api.Models.Users;
using FaisalLeague.Filters;
using FaisalLeague.Maps;
using FaisalLeague.Queries.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FaisalLeague.Server.RestAPI
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILoginQueryProcessor _query;
        private readonly IAutoMapper _mapper;

        public LoginController(ILoginQueryProcessor query, IAutoMapper mapper)
        {
            _query = query;
            _mapper = mapper;
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
        [Authorize]
        public async Task ChangePassword([FromBody]ChangeUserPasswordModel requestModel)
        {
            await _query.ChangePassword(requestModel);
        }
    }
}