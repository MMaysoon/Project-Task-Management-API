using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProjectManagement.Application.Dtos.Auth;
using ProjectManagement.Application.IServices;
using ProjectManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagement.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;
       

        public AuthService(UserManager<ApplicationUser> userManager, IJwtService jwtService, ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<AuthResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            var email = loginDTO.Email.ToLowerInvariant();

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                throw new Exception("Invalid email or password");

            var checkPassword = await _userManager.CheckPasswordAsync(user, loginDTO.Password);

            if (!checkPassword)
                throw new Exception("Invalid email or password");

            var roles = await _userManager.GetRolesAsync(user);

            var token = _jwtService.GenerateToken(user, roles);

            return new AuthResponseDTO
            {
                Token = token,
                Email = user.Email,
                UserName = user.UserName
            };
        }

        public async Task<AuthResponseDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            var email = registerDTO.Email.ToLowerInvariant();

            var existingUser = await _userManager.FindByEmailAsync(email);

            if (existingUser != null)
                throw new Exception("Email is already registered!");

            var user = new ApplicationUser
            {
                UserName = registerDTO.UserName,
                Email = email
            };

            // 1. create user
            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            // 2. add role
            await _userManager.AddToRoleAsync(user, "User");

            // 3. get roles 
            var roles = await _userManager.GetRolesAsync(user);

            // 4. gererate token
            var token = _jwtService.GenerateToken(user, roles);

            return new AuthResponseDTO
            {
                Token = token,
                Email = user.Email,
                UserName = user.UserName
            };
        }
    }
}
