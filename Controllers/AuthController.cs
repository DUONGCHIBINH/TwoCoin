using TwoCoinApi.Models;
using TwoCoinApi.Repos.Interface;
using TwoCoinApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TwoCoinApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthManager _authManager;
        public AuthController(AuthManager authManager)
        {
            _authManager = authManager;
        }

        [AllowAnonymous]
        [HttpGet("get")]
        public IActionResult redisGet()
        {
            //var ob = _dBStatic.Repository.GetQuery<VN_BenhLy>().ToList();
            return Ok("");
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] LoginModel LoginModel)
        {
            //var userExist =  UserConstants.Users.FirstOrDefault(o => o.Username.ToLower() == LoginModel.Username.ToLower());
            //if (userExist != null)
            //{
            //    return BadRequest("User đã tồn tại!");
            //}
            //thêm tài khoản ...
            return Ok("Đăng ký thành công");
        }

        [AllowAnonymous]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResultModel))]
        [HttpPost("login")]
        public ActionResult<AuthResultModel> Login([FromBody] LoginModel LoginModel)
        {
            var result = _authManager.Login(LoginModel);
            if (result is null) return NotFound("User not found");
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] TokenParam token)
        {
            var result = _authManager.RefreshToken(token);
            if (result is null) return BadRequest("Token invalid!");
            return Ok(result);
        }

        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout(string userName)
        {
            bool result = _authManager.Logout(userName);
            if (!result) return BadRequest("LogOut failed");
            return Ok();
        }
    }
}
