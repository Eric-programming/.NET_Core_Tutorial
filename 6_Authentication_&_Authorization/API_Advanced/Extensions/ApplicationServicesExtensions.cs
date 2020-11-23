using API_Advanced.Data.Repo;
using API_Advanced.Models.Interfaces;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace API_Advanced.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            return services;
        }
    }
}