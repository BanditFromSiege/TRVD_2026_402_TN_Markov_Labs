using Microsoft.AspNetCore.Mvc;
using SimpleLibrary.Application.Services;
using SimpleLibrary.Infrastructure.Models;
using SimpleLibrary.REST.Models;

namespace SimpleLibrary.REST.Controllers
{
    /* Lab4
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseModel>> Login([FromBody] AuthLoginRequestModel request)
        {
            var (user, token) = await _authService.SignInAsync(request.Email, request.Password);
            if (user == null || token == null)
                return Unauthorized("Invalid email or password");

            var response = new AuthResponseModel
            {
                Token = token,
                Email = user.Email,
                FullName = user.FullName
            };

            return Ok(response);
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseModel>> Register([FromBody] AuthRegisterRequestModel request)
        {
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                RoleId = 1
            };

            var createdUser = await _authService.SignUpAsync(user, request.Password);
            if (createdUser == null)
                return BadRequest("User already exists");

            var (newUser, token) = await _authService.SignInAsync(createdUser.Email, request.Password);

            var response = new AuthResponseModel
            {
                Token = token!,
                Email = newUser!.Email,
                FullName = newUser.FullName
            };

            return Ok(response);
        }
    }
    */
}