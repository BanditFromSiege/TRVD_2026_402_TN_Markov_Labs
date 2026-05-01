using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleLibrary.Application.Services;
using SimpleLibrary.Infrastructure.Models;
using SimpleLibrary.REST.Models;
using System.Security.Claims;

namespace SimpleLibrary.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserResponseModel>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users.Select(MapToResponse));
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponseModel>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(MapToResponse(user));
        }

        // POST: api/users
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponseModel>> CreateUser([FromBody] UserCreateModel model)
        {
            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                RoleId = model.RoleId
            };

            var created = await _userService.CreateUserAsync(user, model.Password);

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = created.Id },
                MapToResponse(created));
        }

        // PUT: api/users/{id}/role
        [HttpPut("{id}/role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UserRoleUpdateModel model)
        {
            var validRoleIds = new[] { 1, 2, 3 };

            if (!validRoleIds.Contains(model.RoleId))
                return BadRequest("Invalid role ID");

            var updated = await _userService.UpdateUserRoleAsync(id, model.RoleId);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // GET: api/users/me
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserResponseModel>> GetMyProfile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(MapToResponse(user));
        }

        // PUT: api/users/me
        [HttpPut("me")]
        [Authorize]
        public async Task<IActionResult> UpdateMyProfile([FromBody] UserProfileUpdateModel model)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var updated = await _userService.UpdateProfileAsync(
                userId,
                model.FullName,
                model.Email,
                model.Password);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        private static UserResponseModel MapToResponse(User user)
        {
            return new UserResponseModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role?.Name ?? "",
                CreatedAt = user.CreatedAt
            };
        }
    }
}