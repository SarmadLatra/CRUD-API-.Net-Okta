
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CRUD_API.Services.Interfaces;
using CRUD_API.Services.Implementation;
using CRUD_API.TokenServices;

namespace Genius.User.Api.Extensions
{
    public static class IServicesCollectionExtension
    {
        public static void AddDependency(this IServiceCollection services)
        {
            services.AddSingleton<IOktaToken, OktaTokenService>();
            services.AddScoped<IEmployee, EmplyeeService>();
        }
    }
}