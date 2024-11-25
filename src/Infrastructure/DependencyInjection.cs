using System.Text;
using Application.Abstractions;
using Application.Implementations;
using Infrastructure.Configuration;
using Infrastructure.Persistance;
using Infrastructure.Repository;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            //configuration.GetConnectionString("StreamingDb"))
            options.UseNpgsql("Server=dpg-csvjh0btq21c73eq37tg-a.frankfurt-postgres.render.com;Port=5432;Database=streaming_c0jj;User Id=streaming_c0jj_user;Password=KlZhCRuEE7jgY1HDQFMm4Vxejs8fk2RK;"));
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? ""))
            };
        });
        
        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
        services.AddScoped<ISoundtrackRepository, SoundtrackRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();


        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Jwt));
        
        return services;
    }
}