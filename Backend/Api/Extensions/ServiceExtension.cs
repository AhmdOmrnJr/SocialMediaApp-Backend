using Api.Data;
using Api.Entities;
using Api.Services;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.Design;

namespace Api.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Add repositories
            services.AddScoped<IAuthService, AuthService>();
            services.AddIdentity<AppUser, AppRole>()
                    .AddEntityFrameworkStores<DataContext>()
                    .AddDefaultTokenProviders();

            return services;
        }
    }
}