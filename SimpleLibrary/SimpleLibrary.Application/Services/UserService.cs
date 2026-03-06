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

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return null;

            return user;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            return user;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users;
        }

        public async Task<User> CreateUserAsync(User user, string password)
        {
            //For lab4
            //user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.PasswordHash = password;
            user.CreatedAt = DateTime.UtcNow;

            await _userRepository.AddAsync(user);

            user.Role = await _userRepository.GetRoleByIdAsync(user.RoleId)
                    ?? throw new Exception("Role with this Id not found");

            return user;
        }

        public async Task<bool> UpdateUserAsync(int id, User updatedUser)
        {
            var existingUser = await _userRepository.GetByIdAsync(id);
            if (existingUser == null)
                return false;

            existingUser.FullName = updatedUser.FullName;
            existingUser.Email = updatedUser.Email;

            await _userRepository.UpdateAsync(existingUser);
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