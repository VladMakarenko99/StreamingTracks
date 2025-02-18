using System.Text;
using Amazon.Runtime;
using Amazon.S3;
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
        var connectionString = Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_StreamingDb") 
                               ?? configuration.GetConnectionString("StreamingDb");
        
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        
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
        services.AddScoped<ICacheService, CacheService>();
        services.AddHostedService<BackupDeletionService>();
        services.AddScoped<IAwsS3Service, AwsS3Service>();
        
        var awsAccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID") 
                           ?? configuration["AWS:AccessKey"];

        var awsSecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY") 
                           ?? configuration["AWS:SecretKey"];

        var awsRegion = Environment.GetEnvironmentVariable("AWS_REGION") 
                        ?? configuration["AWS:Region"];

        var awsOptions = configuration.GetAWSOptions();
        awsOptions.Credentials = new BasicAWSCredentials(awsAccessKey, awsSecretKey);

        services.AddDefaultAWSOptions(awsOptions);
        services.AddAWSService<IAmazonS3>();
        
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.Jwt));
        
        return services;
    }
}