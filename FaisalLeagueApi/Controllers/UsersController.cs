using System;
using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Models.Common;
using FaisalLeague.Api.Models.Expenses;
using FaisalLeague.Api.Models.Users;
using FaisalLeague.Data.Access.Constants;
using FaisalLeague.Data.Model;
using FaisalLeagueApi.Filters;
using FaisalLeagueApi.Maps;
using FaisalLeague.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FaisalLeagueApi.Helpers;
using Microsoft.AspNetCore.Http;
using FaisalLeague.Api.Common.Exceptions;
using FaisalLeague.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoQueryable.AspNetCore.Filter.FilterAttributes;

namespace FaisalLeagueApi.Controllers
{
    [Route("api/[controller]")]
    
    public class UsersController : Controller
    {
        private readonly IUsersQueryProcessor _query;
        private readonly IMapper _mapper;
        private readonly IImageHandler _imageHandler;
        private readonly ISecurityContext _context;
        private readonly ILeaguesQueryProcessor _leaguesQueryProcessor;

        public UsersController(IUsersQueryProcessor query, IMapper mapper, IImageHandler imageHandler, ISecurityContext context, ILeaguesQueryProcessor leaguesQueryProcessor)
        {
            _query = query;
            _mapper = mapper;
            _imageHandler = imageHandler;
            _context = context;
            _leaguesQueryProcessor = leaguesQueryProcessor;
        }
        /// <summary>
        /// Get Queryable list of users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AutoQueryable]
        public IQueryable<QueryableUserModel> Get()
        {
            var result = _query.Get();
            var models = result.ProjectTo<QueryableUserModel>(_mapper.ConfigurationProvider);
            return models;
        }

        [HttpGet("{id}")]
        public UserModel Get(int id)
        {
            var item = _query.Get(id);
            var model = _mapper.Map<UserModel>(item);
            return model;
        }

        //[HttpPost]
        //[ValidateModel]
        //public async Task<UserModel> Post([FromBody]CreateUserModel requestModel)
        //{
        //    var item = await _query.Create(requestModel);
        //    var model = _mapper.Map<UserModel>(item);
        //    return model;
        //}
        [Authorize(Roles="Administrator")]
        [HttpPost("{id}/password")]
        [ValidateModel]
        public async Task ChangePassword(int id,[FromBody]ChangeUserPasswordModel requestModel)
        {
            await _query.ChangePassword(id, requestModel);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPut("{id}")]
        [ValidateModel]
        public async Task<UserModel> Put(int id, [FromBody]UpdateUserModel requestModel)
        {
            var item = await _query.Update(id, requestModel);
            var model = _mapper.Map<UserModel>(item);
            return model;
        }

        [Authorize(Roles = "Administrator")]
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _query.Delete(id);
        }

        /// <summary>
        /// Uplaods an image to the server.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// 
        [Route("{id}/SetUserImage")]
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<UserModel> SetUserImage(int id)
        {
            IFormFile file;
            if (Request.Form.Files.Any())
                file = Request.Form.Files.Single();
            else
                throw new BadRequestException("no files");
            string imageId= await _imageHandler.UploadImage(file);
            var user = await _query.SetUserImage(id, imageId);
            var model = _mapper.Map<UserModel>(user);

            return model;
        }


        /// <summary>
        /// Set current user profile image
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Current/SetImage")]
        public async Task<UserModel> SetImage()
        {
            IFormFile file;
            if (Request.Form.Files.Any())
                file = Request.Form.Files.Single();
            else
                throw new BadRequestException("no files");
            string imageId = await _imageHandler.UploadImage(file);
            var user = await _query.SetUserImage(_context.User.Id, imageId);
            var model = _mapper.Map<UserModel>(user);

            return model;
        }

    }
}