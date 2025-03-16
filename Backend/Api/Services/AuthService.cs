using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Services
{
    public class AuthService(DataContext dpContext, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IOptions<JWT> jwt) : IAuthService
    {
        private readonly JWT _jwt = jwt.Value;

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

            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));

            var token = new JwtSecurityToken
            (
                audience: _jwt.Audience,
                issuer: _jwt.Issuer,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ResultDto<RegisterDto>> RegisterAsync(RegisterDto model)
        {
            var exist = await userManager.FindByEmailAsync(model.Email);

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
                UserName = model.FirstName + model.LastName,
                KnownAs = model.NickName,
                Gender = model.Gender,
                DateOfBirth = model.DateOfBirth,
                Country = model.Country,
                City = model.City,
            };

            var result = await userManager.CreateAsync(user, model.Password);
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
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return new ResultDto<string>
                {
                    IsSuccess = false,
                    Message = "Invalid email"
                };
            }

            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
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
                Message = "Logged in Successfully",
                Result = await CreateTokenAsync(user, userManager)
            };
        }

        public async Task<ResultDto<IEnumerable<AppUser?>>> GetAllUsers()
        {
            var users = await dpContext.Users.ToListAsync();

            if (users is null)
            {
                return new ResultDto<IEnumerable<AppUser?>>
                {
                    IsSuccess = false,
                    Message = "no Users Found",
                };
            }
            
            return new ResultDto<IEnumerable<AppUser?>>
            {
                IsSuccess = true,
                Message = "Users Fetched Successfully",
                Result = users
            };
        }

        public async Task<ResultDto<AppUser?>> GetUser(Guid Id)
        {
            var user = await dpContext.Users.FindAsync(Id);

            if (user is null)
            {
                return new ResultDto<AppUser?>
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            return new ResultDto<AppUser?>
            {
                IsSuccess =true,
                Message = "User is Found",
                Result = user
            };
        }
    }
}
