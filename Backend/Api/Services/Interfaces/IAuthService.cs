using Api.DTOs;
using Api.Entities;
using Microsoft.AspNetCore.Identity;

namespace Api.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager);
        Task<ResultDto<RegisterDto>> RegisterAsync(RegisterDto model);
        Task<ResultDto<string>> LoginAsync(LoginDto model);
        Task<ResultDto<IEnumerable<AppUser?>>> GetAllUsers();
        Task<ResultDto<AppUser?>> GetUser(Guid Id);
    }
}
