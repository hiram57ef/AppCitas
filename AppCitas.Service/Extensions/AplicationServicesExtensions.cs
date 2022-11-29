using AppCitas.Service.Data;
using AppCitas.Service.Helpers;
using AppCitas.Service.interfaces;
using AppCitas.Service.services;
using Microsoft.EntityFrameworkCore;

namespace AppCitas.Service.Extensions;

public static class AplicationServicesExtensions
{
    public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<ITokenServices, tokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        return services;
    }
}
