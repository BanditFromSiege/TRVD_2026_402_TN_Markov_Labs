using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using SimpleLibrary.Infrastructure.Models;
using SimpleLibrary.Infrastructure.Repositories;

namespace SimpleLibrary.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly string _jwtSecret = "S2754_SUPER_SECRET_KEY_123456789";

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<(User? User, string? Token)> SignInAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
                return (null, null);

            bool validPassword = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

            if (!validPassword)
                return (null, null);

            var token = GenerateJwtToken(user);

            return (user, token);
        }

        public async Task<User?> SignUpAsync(User user, string password)
        {
            var existingUser = await _userRepository.GetByEmailAsync(user.Email);

            if (existingUser != null)
                return null;

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            user.CreatedAt = DateTime.UtcNow;

            await _userRepository.AddAsync(user);

            user.Role = await _userRepository.GetRoleByIdAsync(user.RoleId)
                ?? throw new Exception("Role not found");

            return user;
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                }),

                Expires = DateTime.UtcNow.AddMinutes(60),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}