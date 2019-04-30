using System;
using System.Linq;
using System.Threading.Tasks;
using FaisalLeague.Api.Common;
using FaisalLeague.Api.Common.Exceptions;
using FaisalLeague.Api.Models.Common;
using FaisalLeague.Api.Models.Users;
using FaisalLeague.Data.Access.DAL;
using FaisalLeague.Data.Access.Helpers;
using FaisalLeague.Data.Model;
//using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FaisalLeague.Queries
{
    public class UsersQueryProcessor : IUsersQueryProcessor
    {
        private readonly IUnitOfWork _uow;


        public UsersQueryProcessor(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IQueryable<User> Get()
        {
            var query = GetQuery();

            return query;
        }

        private IQueryable<User> GetQuery()
        {
            return _uow.Query<User>()
                .Where(x => !x.IsDeleted).Include(x=>x.City)
                .Include(x=>x.UserSeasonLeagues)
                .Include(x => x.Roles)
                    .ThenInclude(x => x.Role);
        }

        public User Get(int id)
        {
            var user = GetQuery().FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new NotFoundException("User is not found");
            }

            return user;
        }

        public async Task<User> Create(CreateUserModel model)
        {
            var username = model.Username.Trim();

            if (GetQuery().Any(u => u.Username == username))
            {
                throw new BadRequestException("The username is already in use");
            }

            var user = new User
            {
                Username = model.Username.Trim(),
                Password = model.Password.Trim().WithBCrypt(),
                FirstName = model.FirstName.Trim(),
                MiddleName = model.MiddleName.Trim(),
                LastName = model.LastName.Trim(),
                CityId = model.CityId,
                DOB = DateTime.ParseExact(model.DOB,GlobalSettings.DATE_FORMAT, System.Globalization.CultureInfo.InvariantCulture),
                Email = model.Email.Trim().ToLower(),
                Mobile = model.Mobile.Trim()
                
            };

            //AddUserRoles(user, model.Roles);

            _uow.Add(user);
            await _uow.CommitAsync();

            return user;
        }

        private void AddUserRoles(User user, string[] roleNames)
        {
            user.Roles.Clear();

            foreach (var roleName in roleNames)
            {
                var role = _uow.Query<Role>().FirstOrDefault(x => x.Name == roleName);

                if (role == null)
                {
                    throw new NotFoundException($"Role - {roleName} is not found");
                }

                user.Roles.Add(new UserRole{User = user, Role = role});
            }
        }

        public async Task<User> Update(int id, UpdateUserModel model)
        {
            var user = GetQuery().FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new NotFoundException("User is not found");
            }

            user.Username = model.Username;
            user.FirstName = model.FirstName;
            user.MiddleName = model.MiddleName;
            user.LastName = model.LastName;
            user.CityId = model.CityId;
            user.DOB = DateTime.Parse(model.DOB, System.Globalization.CultureInfo.InvariantCulture);
            user.Email = model.Email;
            user.Mobile = model.Mobile;
            //AddUserRoles(user, model.Roles);

            await _uow.CommitAsync();
            return user;
        }

        public async Task Delete(int id)
        {
            var user = GetQuery().FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                throw new NotFoundException("User is not found");
            }

            if (user.IsDeleted) return;

            user.IsDeleted = true;
            await _uow.CommitAsync();
        }

        public async Task ChangePassword(int id, ChangeUserPasswordModel model)
        {
            var user = Get(id);
            user.Password = model.Password.WithBCrypt();
            await _uow.CommitAsync();
        }

        public async Task<User> SetUserImage(int id, string image)
        {
            var user = GetQuery().FirstOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new NotFoundException("User is not found");
            }

            user.Image = image;
            
            await _uow.CommitAsync();
            return user;
        }

        
    }
}