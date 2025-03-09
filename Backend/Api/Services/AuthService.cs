using Api.DTOs;
using Api.Entities;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthService(IConfiguration configuration, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
        {
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.GivenName, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken
            (
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ResultDto<RegisterDto>> RegisterAsync(RegisterDto model)
        {
            var exist = await _userManager.FindByEmailAsync(model.Email);

            if (exist != null)
            {
                return new ResultDto<RegisterDto>
                {
                    IsSuccess = false,
                    Message = "This Email already exists"
                };
            }

            var user = new AppUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                UserName = model.FirstName + model.LastName
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded == false)
            {
                return new ResultDto<RegisterDto>
                {
                    IsSuccess = false,
                    Message = "Failed To Register"
                };
            }

            return new ResultDto<RegisterDto>
            {
                IsSuccess = true,
                Message = "User Registered Successfully"
            };
        }

        public async Task<ResultDto<string>> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ResultDto<string>
                {
                    IsSuccess = false,
                    Message = "Invalid email"
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (result.Succeeded == false)
            {
                return new ResultDto<string>
                {
                    IsSuccess = false,
                    Message = "User is not found"
                };
            }

            return new ResultDto<string>
            {
                IsSuccess = true,
                Message = "Logged in Succeedded",
                Entity = await CreateTokenAsync(user, _userManager)
            };
        }
    }

}
