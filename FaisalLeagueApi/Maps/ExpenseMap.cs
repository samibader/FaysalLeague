using System.Linq;
using AutoMapper;
using FaisalLeague.Api.Models.Expenses;
using FaisalLeague.Api.Models.Users;
using FaisalLeague.Data.Model;

namespace FaisalLeagueApi.Maps
{
    public class ExpenseMap : IAutoMapperTypeConfigurator
    {
        public void Configure(IMapperConfigurationExpression configuration)
        {
            var map = configuration.CreateMap<Expense, ExpenseModel>();
            map.ForMember(x => x.Username, x => x.MapFrom(y => y.User.FirstName + " " + y.User.LastName));
        }
    }
}