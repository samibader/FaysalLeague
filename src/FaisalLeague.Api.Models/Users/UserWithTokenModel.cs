using System;

namespace FaisalLeague.Api.Models.Users
{
  public class UserWithTokenModel
  {
      public string Token { get; set; }
      public UserModel User { get; set; }
      public string ExpiresAt { get; set; }
  }
}
