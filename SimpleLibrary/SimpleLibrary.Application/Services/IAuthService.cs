using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleLibrary.Infrastructure.Models;

namespace SimpleLibrary.Application.Services
{
    public interface IAuthService
    {
        Task<(User? User, string? Token)> SignInAsync(string email, string password);
        Task<User?> SignUpAsync(User user, string password);
    }
}