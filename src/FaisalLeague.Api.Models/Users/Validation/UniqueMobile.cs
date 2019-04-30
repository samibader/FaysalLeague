using FaisalLeague.Api.Models.Login;
using FaisalLeague.Data.Access.DAL;
using FaisalLeague.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Linq;

namespace FaisalLeague.Api.Models.Users.Validation
{
    public class UniqueMobile : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (RegisterModel)validationContext.ObjectInstance;

            var uow = (IUnitOfWork)validationContext
                         .GetService(typeof(IUnitOfWork));

            var user = (from u in uow.Query<User>()
                        where u.Mobile.ToLower() == model.Mobile.ToLower()
                        select u).FirstOrDefault();

            if (user == null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Mobile already taken!");
        }
    }
}
