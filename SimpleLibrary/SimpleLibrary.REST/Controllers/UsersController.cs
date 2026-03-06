using Microsoft.AspNetCore.Mvc;
using SimpleLibrary.Application.Services;
using SimpleLibrary.Infrastructure.Models;
using SimpleLibrary.REST.Models;

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
        public async Task<ActionResult<IEnumerable<UserResponseModel>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();

            var result = users.Select(MapToResponse);

            return Ok(result);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseModel>> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(MapToResponse(user));
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserResponseModel>> CreateUser([FromBody] UserCreateModel model)
        {
            var validRoleIds = new[] { 1, 2, 3 }; // 1 = User, 2 = Librarian, 3 = Admin
            if (!validRoleIds.Contains(model.RoleId))
            {
                return BadRequest("Invalid role");
            }

            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                RoleId = model.RoleId,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _userService.CreateUserAsync(user, model.Password);

            var response = new UserResponseModel
            {
                Id = created.Id,
                FullName = created.FullName,
                Email = created.Email,
                Role = created.Role.Name,
                CreatedAt = created.CreatedAt
            };

            return CreatedAtAction(
                nameof(GetUserById),
                new { id = created.Id },
                response
            );
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateModel model)
        {
            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email
            };

            var updated = await _userService.UpdateUserAsync(id, user);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
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