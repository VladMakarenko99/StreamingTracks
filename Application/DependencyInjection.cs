using System.Reflection;
using Application.Abstractions;
using Application.Implementations;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(DependencyInjection).Assembly;
        
        services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(
                assembly));
        
        services.AddValidatorsFromAssembly(assembly);
        services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
        
        return services;
    }
}