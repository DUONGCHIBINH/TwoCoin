using TwoCoinApi.Models;
using TwoCoinApi.Repos.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TwoCoinApi
{
    public class TwoCoinUtil
    {
        public static UserModel? getCurrentUser(HttpContext HttpContext, IUserRepository userReopository)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return null;

            return userReopository.GetById(userId).GetInfo();


        }
    }
}
