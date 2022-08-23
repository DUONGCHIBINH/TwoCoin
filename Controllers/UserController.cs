using TwoCoinApi.Models;
using TwoCoinApi.Repos.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TwoCoinApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return Ok(_userRepository.GetAll());
            }
            catch
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult test()
        {
            return Ok("test");
        }



        [HttpGet("{id}")]
        public IActionResult GetByUsername(string id)
        {
            try
            {
                var user = _userRepository.GetById(id);
                return user == null
                    ? Ok()
                    : NotFound();
            }
            catch
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost]
        public IActionResult Add([FromBody] UserModel user)
        {
            try
            {
                var data = _userRepository.Add(user);
                return Ok(data);
            }
            catch
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] UserModel user)
        {
            try
            {
                if (id != user.Username) return BadRequest();

                _userRepository.Update(user);
                return NoContent();
            }
            catch
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            try
            {

                _userRepository.Delete(id);
                return NoContent();
            }
            catch
            {

                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}
