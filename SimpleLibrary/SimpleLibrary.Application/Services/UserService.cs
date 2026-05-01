using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLibrary.Infrastructure.Models;
using SimpleLibrary.Infrastructure.Repositories;

namespace SimpleLibrary.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.CreatedAt = DateTime.UtcNow;

            await _userRepository.AddAsync(user);

            user.Role = await _userRepository.GetRoleByIdAsync(user.RoleId)
                ?? throw new Exception("Role not found");

            return user;
        }

        public async Task<bool> UpdateProfileAsync(int userId, string fullName, string email, string? password)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return false;

            user.FullName = fullName;
            user.Email = email;

            if (!string.IsNullOrWhiteSpace(password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            }

            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> UpdateUserRoleAsync(int userId, int roleId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return false;

            var role = await _userRepository.GetRoleByIdAsync(roleId);

            if (role == null)
                return false;

            user.RoleId = role.Id;
            user.Role = role;

            await _userRepository.UpdateAsync(user);

            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return false;

            await _userRepository.DeleteAsync(user);

            return true;
        }
    }
}